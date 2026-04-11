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

namespace SiliconLife.Default.Web.Models;

public class SocialMedia
{
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string IconContent { get; set; } = string.Empty;
}

public class AboutViewModel : ViewModelBase
{
    public string Version { get; set; } = "1.0.0";
    public string Author => Localization.AboutAuthorName;
    public string License { get; set; } = "Apache License 2.0";
    public string GitHubUrl { get; set; } = "https://github.com/akimoto-akira/SiliconLifeCollective";
    public string GiteeUrl { get; set; } = "https://gitee.com/hoshinokennji/SiliconLifeCollective";
    public List<SocialMedia> SocialMediaList { get; set; } = new()
    {
        new SocialMedia
        {
            Platform = "Bilibili",
            Url = "https://space.bilibili.com/617827040",
            IconContent = "<svg viewBox=\"0 0 24 24\" fill=\"currentColor\"><path d=\"M17.813 4.653h.854c1.51.054 2.769.578 3.773 1.574 1.004.995 1.524 2.249 1.56 3.76v7.36c-.036 1.51-.556 2.769-1.56 3.773s-2.262 1.524-3.773 1.56H5.333c-1.51-.036-2.769-.556-3.773-1.56S.036 18.858 0 17.347v-7.36c.036-1.511.556-2.765 1.56-3.76 1.004-.996 2.262-1.52 3.773-1.574h.774l-1.174-1.12a1.234 1.234 0 0 1-.373-.906c0-.356.124-.658.373-.907l.027-.027c.267-.249.573-.373.92-.373.347 0 .653.124.92.373L9.653 4.44c.071.071.134.142.187.213h4.267a.836.836 0 0 1 .16-.213l2.853-2.747c.267-.249.573-.373.92-.373.347 0 .662.151.929.4.267.249.391.551.391.907 0 .355-.124.657-.373.906zM5.333 7.24c-.746.018-1.373.276-1.88.773-.506.498-.769 1.13-.786 1.894v7.52c.017.764.28 1.395.786 1.893.507.498 1.134.756 1.88.773h13.334c.746-.017 1.373-.275 1.88-.773.506-.498.769-1.129.786-1.893v-7.52c-.017-.765-.28-1.396-.786-1.894-.507-.497-1.134-.755-1.88-.773zM8 11.107c.373 0 .684.124.933.373.25.249.383.569.4.96v1.173c-.017.391-.15.711-.4.96-.249.25-.56.374-.933.374s-.684-.125-.933-.374c-.25-.249-.383-.569-.4-.96V12.44c0-.373.129-.689.386-.947.258-.257.574-.386.947-.386zm8 0c.373 0 .684.124.933.373.25.249.383.569.4.96v1.173c-.017.391-.15.711-.4.96-.249.25-.56.374-.933.374s-.684-.125-.933-.374c-.25-.249-.383-.569-.4-.96V12.44c.017-.391.15-.711.4-.96.249-.249.56-.373.933-.373z\"/></svg>"
        },
        new SocialMedia
        {
            Platform = "YouTube",
            Url = "https://www.youtube.com/@hoshinokennji",
            IconContent = "<svg viewBox=\"0 0 24 24\" fill=\"currentColor\"><path d=\"M23.498 6.186a3.016 3.016 0 0 0-2.122-2.136C19.505 3.545 12 3.545 12 3.545s-7.505 0-9.377.505A3.017 3.017 0 0 0 .502 6.186C0 8.07 0 12 0 12s0 3.93.502 5.814a3.016 3.016 0 0 0 2.122 2.136c1.871.505 9.376.505 9.376.505s7.505 0 9.377-.505a3.015 3.015 0 0 0 2.122-2.136C24 15.93 24 12 24 12s0-3.93-.502-5.814zM9.545 15.568V8.432L15.818 12l-6.273 3.568z\"/></svg>"
        },
        new SocialMedia
        {
            Platform = "X",
            Url = "https://x.com/SuzukiKennji4cn",
            IconContent = "<svg viewBox=\"0 0 24 24\" fill=\"currentColor\"><path d=\"M18.244 2.25h3.308l-7.227 8.26 8.502 11.24H16.17l-5.214-6.817L4.99 21.75H1.68l7.73-8.835L1.254 2.25H8.08l4.713 6.231zm-1.161 17.52h1.833L7.084 4.126H5.117z\"/></svg>"
        },
        new SocialMedia
        {
            Platform = "Kuaishou",
            Url = "https://www.kuaishou.com/profile/3xt8zdgzzt32mpw",
            IconContent = "<svg viewBox=\"0 0 81 31\" xmlns=\"http://www.w3.org/2000/svg\"><g fill=\"none\" fill-rule=\"evenodd\"><path d=\"M46.872 6.105h-.032l-2.027.003.002 1.886h-3.901a.64.64 0 0 0-.644.603l-.001.032v2.008h4.548v3.603l-.007 1.463-.003.163-.004.159c-.008.267-.02.603-.039.853l-.011.149-.03.346h-4.73a.64.64 0 0 0-.645.604v2.039h5.212c-.958 2.675-2.749 4.425-5.213 5.39l.645 2.173c.063.21.372.27.52.21 1.769-.698 4.89-2.459 6.474-6.655 1.402 3.99 4.65 5.85 6.125 6.511l.124.055c.491.213.7 0 .779-.264l.607-2.03c-.997-.395-3.95-1.672-5.203-5.39H54.005a.64.64 0 0 0 .62-.603v-2.04h-1.28V8.63a.64.64 0 0 0-.614-.634l-.032-.001h-5.212l-.002-1.268V6.705a.638.638 0 0 0-.58-.598l-.033-.002zm-9.504 0H35.34v21.017c0 .34.271.618.612.634l.032.001 2.028-.012V14.987h1.609a.64.64 0 0 0 .62-.603v-2.04h-2.23V6.716a.64.64 0 0 0-.612-.61h-.032zm38.923.252c-.355.507-1.04 1.083-2.048 1.095h-15.6V9.46c0 .329.254.6.58.631l.033.003h8.243v3.463h-7.462a.64.64 0 0 0-.644.604v.032l-.001 2.007h8.107v3.15h-9.78a.64.64 0 0 0-.645.603v2.039h10.425v2.345l-.002.043-.002.022-.004.036a.795.795 0 0 1-.755.674l-.037.001H63.1l.583 2.157a.66.66 0 0 0 .56.483l.033.002h2.509l.055-.002.059-.003c1.814-.102 3.242-1.574 3.272-3.37v-2.388h9.434a.64.64 0 0 0 .62-.603v-2.04H70.171V16.2h7.032a.64.64 0 0 0 .62-.603l.001-.031v-2.008H70.17v-3.463c.885.005 2.223.004 4.014-.004 2.918.012 4.411-2.102 4.808-3.076l-1.93-.825c-.367-.157-.638-.024-.772.167zM33.574 8.98h-2.028v7.203l-.002.23-.002.145a16.74 16.74 0 0 1-.761 4.544.632.632 0 0 0 .354.765l.031.012 1.97.658c.678-1.875 1.041-3.843 1.08-5.99l.001-.174.001-.17.002-.708-.002-.001v-5.91a.64.64 0 0 0-.58-.6l-.032-.003-.032-.001zm13.916 1.656h3.182v6.737h-3.264l.029-.402c.025-.383.04-.86.046-1.279l.001-.18.002-.447.004-4.429z\" fill=\"#FFF\" fill-rule=\"nonzero\"/><path d=\"M21.02 16.34c2.903 0 5.255 2.34 5.255 5.225v4.15c0 2.885-2.352 5.224-5.255 5.224h-7.728a5.255 5.255 0 0 1-4.995-3.597l-4.604 2.294a2.557 2.557 0 0 1-3.423-1.13A2.523 2.523 0 0 1 0 27.37v-7.442c0-1.4 1.142-2.535 2.55-2.535.4 0 .795.093 1.152.273l4.584 2.307a5.254 5.254 0 0 1 5.006-3.632h7.728zm-.033 2.69H13.37a2.62 2.62 0 0 0-2.628 2.569v4.038a2.62 2.62 0 0 0 2.584 2.612h7.617a2.62 2.62 0 0 0 2.627-2.568v-4.039a2.62 2.62 0 0 0-2.583-2.612zM3.4 20.65a.694.694 0 0 0-.695.679v4.678a.697.697 0 0 0 1 .622l.011-.006 4.321-2.191v-1.55l-4.325-2.158a.699.699 0 0 0-.312-.074zM9.815.975A7.114 7.114 0 0 1 16.02 4.59a6.1 6.1 0 0 1 4.15-1.619c3.372 0 6.105 2.718 6.105 6.07 0 3.352-2.733 6.07-6.105 6.07a6.108 6.108 0 0 1-4.934-2.495 7.112 7.112 0 0 1-5.421 2.495c-3.927 0-7.11-3.165-7.11-7.069S5.888.975 9.815.975zm0 2.842c-2.348 0-4.25 1.892-4.25 4.226s1.902 4.226 4.25 4.226c2.347 0 4.25-1.892 4.25-4.226s-1.903-4.226-4.25-4.226zM20.17 5.815a3.236 3.236 0 0 0-3.245 3.227 3.236 3.236 0 0 0 3.245 3.227 3.236 3.236 0 0 0 3.246-3.227 3.236 3.236 0 0 0-3.246-3.227z\" fill=\"#FF4906\"/></g></svg>"
        },
        new SocialMedia
        {
            Platform = "Douyin",
            Url = "https://v.douyin.com/juae_OG64_Y/",
            IconContent = "<svg viewBox=\"0 0 24 24\" fill=\"currentColor\"><path d=\"M19.59 6.69a4.83 4.83 0 0 1-3.77-4.25V2h-3.45v13.67a2.89 2.89 0 0 1-5.2 1.74 2.89 2.89 0 0 1 2.31-4.64 2.93 2.93 0 0 1 .88.13V9.4a6.84 6.84 0 0 0-1-.05A6.33 6.33 0 0 0 5 20.1a6.34 6.34 0 0 0 10.86-4.43v-7a8.16 8.16 0 0 0 4.77 1.52v-3.4a4.85 4.85 0 0 1-1-.1z\"/></svg>"
        },
        new SocialMedia
        {
            Platform = "Xiaohongshu",
            Url = "https://www.xiaohongshu.com/user/profile/62007ec8000000001000b2f9",
            IconContent = "<img src=\"data:image/png;base64,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\" style=\"width:20px;height:20px;\" alt=\"小红书\"/>"
        }
    };
}
