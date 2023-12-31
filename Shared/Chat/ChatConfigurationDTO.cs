﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Chat
{
    public enum ChatConfigurationDTO
    {
        TextOnly,
        Dominant,
        Listening,
        Speech,
        OnlyHuman
    }
    public class ChatConfigurationContainer
    {
        public List<ChatConfigurationDTO> Configurations { get; set; }
    }
}
