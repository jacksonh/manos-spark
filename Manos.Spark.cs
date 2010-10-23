//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//



using Spark;
using Spark.FileSystem;

using System;
using System.Reflection;


namespace Manos.Spark {

	public static class ManosModuleExtensions {

		private static SparkViewEngine engine = null;

		public static void RenderSparkView (this ManosModule module, IManosContext ctx, string template, object data)
		{
			if (engine == null)
				SetupEngine ();

			var descriptor = new SparkViewDescriptor ().AddTemplate (template).AddTemplate ("base.spark");
			var view = (ManosSparkTemplate) engine.CreateInstance (descriptor);

			view.Data = data;

			try {
				view.RenderView (ctx.Response.Writer);
				ctx.Response.Writer.Flush ();
			} catch (Exception e) {
				Console.WriteLine ("Exception while writing template.");
				Console.WriteLine (e);
			} finally {
				engine.ReleaseInstance (view);
			}
		}

		public static void SetupEngine ()
		{
			engine = new SparkViewEngine ();
			engine.DefaultPageBaseType = "Manos.Spark.ManosSparkTemplate";

			var vf = new FileSystemViewFolder ("Templates");
			vf.AddLayoutsPath ("Templates");

			engine.ViewFolder = vf;
		}
	}

	public abstract class ManosSparkTemplate : AbstractSparkView {

		public object Data {
			get;
			set;
		}

		public object Eval (string expression)
		{
			string value = GetValue (expression);
			return value;
		}

		public string Eval (string expression, string format)
		{
			string value = GetValue (expression);
			return String.Format (format, value);
		}

		public string GetValue (string expression)
		{
			PropertyInfo prop = Data.GetType ().GetProperty (expression);

			if (prop == null)
				return String.Empty;

			object value = prop.GetValue (Data, null);
			if (value == null)
				return String.Empty;

			return value.ToString ();
		}

	}
}

