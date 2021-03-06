﻿#region

using System.Collections.Generic;
using db;
using wServer.networking.cliPackets;
using wServer.realm.entities;

#endregion

namespace wServer.networking.handlers
{
    internal class EditAccountListHandler : PacketHandlerBase<EditAccountListPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.EditAccountList; }
        }

        protected override void HandlePacket(Client client, EditAccountListPacket packet)
        {
            Player target;
            if (client.Player.Owner == null) return;
            client.Manager.Logic.AddPendingAction(t => client.Manager.Data.AddPendingAction(db =>
            {
                target = client.Player.Owner.GetEntity(packet.ObjectId) is Player
                    ? client.Player.Owner.GetEntity(packet.ObjectId) as Player
                    : null;
                if (target == null) return;
                switch (packet.AccountListId)
                {
                    case Client.LOCKED_LIST_ID:
                        if (packet.Add)
                            db.AddLock(client.Account.AccountId, target.AccountId);
                        else
                            db.RemoveLock(client.Account.AccountId, target.AccountId);
                        break;
                    case Client.IGNORED_LIST_ID:
                        if (packet.Add)
                            db.AddIgnore(client.Account.AccountId, target.AccountId);
                        else
                            db.RemoveIgnore(client.Account.AccountId, target.AccountId);
                        break;
                }
            }));
        }
    }
}