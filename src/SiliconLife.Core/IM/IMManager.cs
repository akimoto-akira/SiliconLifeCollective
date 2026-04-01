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

namespace SiliconLife.Collective;

public class IMManager
{
    private readonly IIMProvider _imProvider;
    private readonly ChatSystem _chatSystem;
    private readonly SiliconBeingManager _beingManager;
    private bool _isRunning;

    public IMManager(IIMProvider imProvider, ChatSystem chatSystem, SiliconBeingManager beingManager)
    {
        _imProvider = imProvider;
        _chatSystem = chatSystem;
        _beingManager = beingManager;
        _isRunning = false;

        _imProvider.MessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(object? sender, IMMessageEventArgs e)
    {
        ChatMessage msg = e.Message;
        if (msg.ReceiverId == Guid.Empty)
        {
            List<SiliconBeingBase> beings = _beingManager.GetAllBeings();
            if (beings.Count > 0)
            {
                msg.ReceiverId = beings[0].Id;
                _chatSystem.AddMessage(msg.SenderId, msg.ReceiverId, msg.Content);
            }
        }
        else
        {
            _chatSystem.AddMessage(msg.SenderId, msg.ReceiverId, msg.Content);
        }
    }

    public async Task SendMessageAsync(Guid senderId, Guid receiverId, string content)
    {
        await _imProvider.SendMessageAsync(senderId, receiverId, content);
    }

    public async Task StartAsync()
    {
        _isRunning = true;
        await _imProvider.StartAsync();
    }

    public async Task StopAsync()
    {
        _isRunning = false;
        await _imProvider.StopAsync();
    }
}
