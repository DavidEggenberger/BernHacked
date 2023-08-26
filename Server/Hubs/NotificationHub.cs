using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Server.DomainFeatures.ChatAggregate.Domain;
using Shared.Chat;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class NotificationHub : Hub
    {

    }
}
