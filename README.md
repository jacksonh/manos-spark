Manos.Spark
===========

Manos.Spark lets you use the Spark view engine [http://sparkviewengine.com/](http://sparkviewengine.com/) in your Manos web applications.

Installation
------------

You should be able to drop this file into your project and compile it with your app.  You will need to download the Spark.dll library from the Spark website.

Manos.Spark makes a couple of assumptions about your projects layout.  Your base template file must be named base.spark and your templates should be in the Templates directory.

Manos.Spark adds a render method to the ManosModule class, so you should be able to call RenderSparkView from your action methods.  The RenderSparkView method is similar to the RenderTemplate Manos method.  You pass the method the context, path to your template and an object to look up data.  The data lookup is done by property name, this lets you use anonymous types easily.


Example
-------

in SomeAction.cs:

    public void Index (IManosContext ctx)
    {
        this.RenderSparkView (ctx, "index.spark", new {
             Title = "Manos de Mono",
        });
    }

in base.spark:

    <html>
      <title>${#Title}</title>
      <body>
        <use content="body" />
      </body>
    </html>

in index.spark:

   <use master="base.spark" />

   <content name="body">
     <h1>${#Title}</h1>
     Why, Hello thar.
   </content>

