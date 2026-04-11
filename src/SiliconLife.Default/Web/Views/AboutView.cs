// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SiliconLife.Default.Web.Models;

namespace SiliconLife.Default.Web.Views;

public class AboutView : ViewBase
{
    public override string Render(object model)
    {
        var vm = model as AboutViewModel;
        if (vm == null) return string.Empty;
        var body = RenderBody(vm);
        return RenderPage(vm.Skin, vm.Localization.PageTitleAbout, "about", vm.Localization, body, null, GetStyles());
    }

    private static H RenderBody(AboutViewModel vm)
    {
        return H.Div(
            H.Div(
                H.H1(vm.Localization.AboutPageHeader)
            ).Class("page-header"),
            H.Div(
                H.Div(
                    H.H3(vm.Localization.AboutAppName),
                    H.P(vm.Localization.AboutDescription).Class("about-description")
                ).Class("card about-app-card"),
                H.Div(
                    H.Div(
                        H.Span(vm.Localization.AboutVersionLabel + ": ").Class("about-label"),
                        H.Span(vm.Version).Class("about-value")
                    ).Class("about-info-row"),
                    H.Div(
                        H.Span(vm.Localization.AboutAuthorLabel + ": ").Class("about-label"),
                        H.Span(vm.Author).Class("about-value")
                    ).Class("about-info-row"),
                    RenderSocialMediaSection(vm),
                    H.Div(
                        H.Span(vm.Localization.AboutLicenseLabel + ": ").Class("about-label"),
                        H.Span(vm.License).Class("about-value")
                    ).Class("about-info-row"),
                    H.Div(
                        H.Span("").Class("about-label"),
                        H.Div(
                            H.A(vm.Localization.AboutGitHubLink).Href(vm.GitHubUrl).Attr("target", "_blank").Class("btn btn-sm"),
                            H.A(vm.Localization.AboutGiteeLink).Href(vm.GiteeUrl).Attr("target", "_blank").Class("btn btn-sm")
                        ).Class("about-links")
                    ).Class("about-info-row")
                ).Class("card"),
                H.Div(
                    H.P(vm.Localization.AboutCopyright).Class("about-copyright")
                ).Class("about-footer")
            ).Class("page-content")
        );
    }

    private static H RenderSocialMediaSection(AboutViewModel vm)
    {
        if (vm.SocialMediaList == null || vm.SocialMediaList.Count == 0)
            return H.Div();

        var socialItems = vm.SocialMediaList.Select(sm =>
            H.A(
                new RawHtml(sm.IconContent),
                H.Span(vm.Localization.GetSocialMediaName(sm.Platform))
            ).Href(sm.Url).Attr("target", "_blank").Class("about-social-item")
        ).ToArray();

        return H.Div(
            H.Details(
                H.Summary(vm.Localization.AboutSocialMediaLabel).Class("about-summary"),
                H.Div(socialItems).Class("about-social-list")
            ).Class("about-social-section")
        ).Class("about-info-row");
    }

    private static CssBuilder GetStyles()
    {
        return CssBuilder.Create()
            .Selector(".about-app-card")
                .Property("text-align", "center")
            .EndSelector()
            .Selector(".about-description")
                .Property("color", "var(--text-secondary)")
                .Property("line-height", "1.6")
                .Property("margin-top", "12px")
            .EndSelector()
            .Selector(".about-info-row")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("margin-bottom", "12px")
            .EndSelector()
            .Selector(".about-label")
                .Property("font-weight", "600")
                .Property("min-width", "80px")
            .EndSelector()
            .Selector(".about-value")
                .Property("color", "var(--text-secondary)")
            .EndSelector()
            .Selector(".about-links")
                .Property("display", "flex")
                .Property("gap", "10px")
            .EndSelector()
            .Selector(".about-social-section")
                .Property("width", "100%")
            .EndSelector()
            .Selector(".about-summary")
                .Property("cursor", "pointer")
                .Property("font-weight", "600")
                .Property("padding", "8px 0")
                .Property("color", "var(--text-primary)")
                .Property("list-style", "none")
                .Property("outline", "none")
            .EndSelector()
            .Selector(".about-summary::-webkit-details-marker")
                .Property("display", "none")
            .EndSelector()
            .Selector(".about-summary::before")
                .Property("content", "\"▶\"")
                .Property("display", "inline-block")
                .Property("margin-right", "8px")
                .Property("transition", "transform 0.2s")
            .EndSelector()
            .Selector(".about-social-section[open] .about-summary::before")
                .Property("transform", "rotate(90deg)")
            .EndSelector()
            .Selector(".about-social-list")
                .Property("padding", "12px 0 12px 24px")
                .Property("display", "flex")
                .Property("flex-direction", "column")
                .Property("gap", "10px")
            .EndSelector()
            .Selector(".about-social-item")
                .Property("display", "flex")
                .Property("align-items", "center")
                .Property("gap", "10px")
                .Property("color", "var(--text-secondary)")
                .Property("text-decoration", "none")
                .Property("transition", "color 0.2s")
            .EndSelector()
            .Selector(".about-social-item:hover")
                .Property("color", "var(--accent-primary)")
            .EndSelector()
            .Selector(".about-social-item svg")
                .Property("height", "20px")
                .Property("width", "auto")
                .Property("flex-shrink", "0")
            .EndSelector()
            .Selector(".about-social-item img")
                .Property("height", "20px")
                .Property("width", "auto")
                .Property("flex-shrink", "0")
            .EndSelector()
            .Selector(".about-footer")
                .Property("text-align", "center")
                .Property("margin-top", "40px")
                .Property("color", "var(--text-secondary)")
                .Property("font-size", "13px")
            .EndSelector()
            .Selector(".about-copyright")
                .Property("margin", "0")
            .EndSelector();
    }
}
