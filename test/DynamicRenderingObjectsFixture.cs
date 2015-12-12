﻿using System;
using System.Linq;
using TinySite.Models;
using Xunit;

namespace RobMensching.TinySite.Test
{
    public class DynamicRenderingObjectsFixture
    {
        [Fact]
        public void CanReadDynamicRenderingData()
        {
            var sitePath = @"C:\site\";
            var documentsPath = sitePath + @"documents\";
            var layoutsPath = sitePath + @"layouts\";
            var outputPath = sitePath + @"build\";

            var document = new DocumentFile(documentsPath + "foo.html.md", documentsPath, outputPath + "foo.html", outputPath, String.Empty, String.Empty, null, null);
            var layout = new LayoutFile(layoutsPath + "default.cshtml", layoutsPath, String.Empty, null);
            var config = new SiteConfig() { OutputPath = outputPath, Url = "http://example.com", RootUrl = String.Empty, };
            var site = new Site(config, new[] { document }, Enumerable.Empty<StaticFile>(), new LayoutFileCollection(new[] { layout }));

            dynamic data = new DynamicRenderingData(document, layout, site);
            Assert.Equal(outputPath, data.Site.OutputPath);
            Assert.Single(data.Site.Documents);

            foreach (var d in data.Site.Documents)
            {
                Assert.Equal(outputPath + "foo.html", d.OutputPath);
            }
        }
    }
}