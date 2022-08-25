using OTAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Net.Sockets;
using TerrariaApi.Server;
using TShockAPI;

namespace SofDefender
{
    [ApiVersion(2, 1)]
    public class MainPlugin : TerrariaPlugin
    {
        public override string Name => "SofDefender";
        public override string Description => "Defend the attack of sof";
        public override string Author => "Leader";
        public override Version Version => new Version(1, 0, 0, 0);
        public MainPlugin(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
            ServerApi.Hooks.ServerJoin.Register(this, OnServerJoin);
            Hooks.Net.Socket.Accepted += new Hooks.Net.Socket.AcceptedHandler(this.OnSocketAccpet);
        }

        List<ISocket> BlankClients = new List<ISocket>();

        private HookResult OnSocketAccpet(ISocket client)
        {
            //the legnth of the buffer,it should be euqal to the hardware of your server.
            if (BlankClients.Count > 20)
            {
                BlankClients[0].Close();
                BlankClients.RemoveAt(0);
            }
            BlankClients.Add(client);
            return HookResult.Cancel;
        }


        private void OnServerJoin(JoinEventArgs args)
        {
            int index = BlankClients.FindIndex(x => x.GetRemoteAddress() == Netplay.Clients[args.Who].Socket.GetRemoteAddress());
            BlankClients.RemoveAt(index);
        }
    }
}
