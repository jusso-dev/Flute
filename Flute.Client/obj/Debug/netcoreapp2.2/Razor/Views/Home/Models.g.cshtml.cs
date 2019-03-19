#pragma checksum "C:\Users\A_Justin.Midler\Desktop\Flute\Flute.Client\Views\Home\Models.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "36ac1c7c572cbfbdf8a89a206d922baec8cbd413"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Models), @"mvc.1.0.view", @"/Views/Home/Models.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Models.cshtml", typeof(AspNetCore.Views_Home_Models))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\A_Justin.Midler\Desktop\Flute\Flute.Client\Views\_ViewImports.cshtml"
using Flute.Client;

#line default
#line hidden
#line 2 "C:\Users\A_Justin.Midler\Desktop\Flute\Flute.Client\Views\_ViewImports.cshtml"
using Flute.Client.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"36ac1c7c572cbfbdf8a89a206d922baec8cbd413", @"/Views/Home/Models.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"441a5332eb94de4e48381d747ff879f2eba181fb", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Models : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Flute.Client.ViewModels.TrainedModelsViewModel>
    {
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(55, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(57, 40, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "36ac1c7c572cbfbdf8a89a206d922baec8cbd4133358", async() => {
                BeginContext(63, 27, true);
                WriteLiteral("\r\n\t<style>\r\n\t\t\r\n\t</style>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(97, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
            BeginContext(101, 1339, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "36ac1c7c572cbfbdf8a89a206d922baec8cbd4134578", async() => {
                BeginContext(107, 539, true);
                WriteLiteral(@"
	<section style=""margin-top:2%;"">
		<div class=""text-center"">
			<h1>
				Binary Neural Network Trained Models
			</h1>

			<br />

			<p>
				Below, you can see a list of trained Models, as well as an example on how to consume the model.
			</p>
			<br />

			<div class=""col-md-4"" style=""margin-left:auto;margin-right:auto"">
				<table class=""table table-striped"">
					<thead>
						<tr>
							<th scope=""col"">Model ID</th>
							<th scope=""col"">Model Uploaded Time</th>
						</tr>
					</thead>
					<tbody>
");
                EndContext();
#line 32 "C:\Users\A_Justin.Midler\Desktop\Flute\Flute.Client\Views\Home\Models.cshtml"
                         foreach (var item in Model.ListOfTrainedModels)
						{

#line default
#line hidden
                BeginContext(711, 25, true);
                WriteLiteral("\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>");
                EndContext();
                BeginContext(737, 12, false);
#line 35 "C:\Users\A_Justin.Midler\Desktop\Flute\Flute.Client\Views\Home\Models.cshtml"
                               Write(item.ModelId);

#line default
#line hidden
                EndContext();
                BeginContext(749, 19, true);
                WriteLiteral("</td>\r\n\t\t\t\t\t\t\t\t<td>");
                EndContext();
                BeginContext(769, 24, false);
#line 36 "C:\Users\A_Justin.Midler\Desktop\Flute\Flute.Client\Views\Home\Models.cshtml"
                               Write(item.ModelUploadDateTime);

#line default
#line hidden
                EndContext();
                BeginContext(793, 19, true);
                WriteLiteral("</td>\r\n\t\t\t\t\t\t\t\t<td>");
                EndContext();
                BeginContext(813, 22, false);
#line 37 "C:\Users\A_Justin.Midler\Desktop\Flute\Flute.Client\Views\Home\Models.cshtml"
                               Write(item.ModelFriendlyName);

#line default
#line hidden
                EndContext();
                BeginContext(835, 21, true);
                WriteLiteral("</td>\r\n\t\t\t\t\t\t\t</tr>\r\n");
                EndContext();
#line 39 "C:\Users\A_Justin.Midler\Desktop\Flute\Flute.Client\Views\Home\Models.cshtml"
						}

#line default
#line hidden
                BeginContext(865, 55, true);
                WriteLiteral("\t\t\t\t\t</tbody>\r\n\t\t\t\t</table>\r\n\t\t\t</div>\r\n\r\n\t\t\t<br />\r\n\r\n");
                EndContext();
#line 46 "C:\Users\A_Justin.Midler\Desktop\Flute\Flute.Client\Views\Home\Models.cshtml"
             if (!string.IsNullOrEmpty(TempData["Error"]?.ToString()))
			{

#line default
#line hidden
                BeginContext(989, 114, true);
                WriteLiteral("\t\t\t\t<div style=\"margin-top:1%;margin-bottom:1%;\" class=\"col-md-12\">\r\n\t\t\t\t\t<div class=\"alert alert-danger\">\r\n\t\t\t\t\t\t");
                EndContext();
                BeginContext(1104, 29, false);
#line 50 "C:\Users\A_Justin.Midler\Desktop\Flute\Flute.Client\Views\Home\Models.cshtml"
                   Write(TempData["Error"]?.ToString());

#line default
#line hidden
                EndContext();
                BeginContext(1133, 27, true);
                WriteLiteral("\r\n\t\t\t\t\t</div>\r\n\t\t\t\t</div>\r\n");
                EndContext();
#line 53 "C:\Users\A_Justin.Midler\Desktop\Flute\Flute.Client\Views\Home\Models.cshtml"
			}

#line default
#line hidden
                BeginContext(1166, 2, true);
                WriteLiteral("\r\n");
                EndContext();
#line 55 "C:\Users\A_Justin.Midler\Desktop\Flute\Flute.Client\Views\Home\Models.cshtml"
             if (!string.IsNullOrEmpty(TempData["Info"]?.ToString()))
			{

#line default
#line hidden
                BeginContext(1236, 112, true);
                WriteLiteral("\t\t\t\t<div style=\"margin-top:1%;margin-bottom:1%;\" class=\"col-md-12\">\r\n\t\t\t\t\t<div class=\"alert alert-info\">\r\n\t\t\t\t\t\t");
                EndContext();
                BeginContext(1349, 28, false);
#line 59 "C:\Users\A_Justin.Midler\Desktop\Flute\Flute.Client\Views\Home\Models.cshtml"
                   Write(TempData["Info"]?.ToString());

#line default
#line hidden
                EndContext();
                BeginContext(1377, 27, true);
                WriteLiteral("\r\n\t\t\t\t\t</div>\r\n\t\t\t\t</div>\r\n");
                EndContext();
#line 62 "C:\Users\A_Justin.Midler\Desktop\Flute\Flute.Client\Views\Home\Models.cshtml"
			}

#line default
#line hidden
                BeginContext(1410, 23, true);
                WriteLiteral("\t\t</div>\r\n\t</section>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Flute.Client.ViewModels.TrainedModelsViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
