
function init(completeCallback)
{
	completeCallback();
}

var FriendsInfoCache = IMCore.Session.GetGlobal("FriendsInfoCache");

Module.GetFriends = function(callback,userCache)
{
	FriendsInfoCache.GetFriends(callback,userCache);
}

Module.SendAddFriendRequest = function(callback, peer, info)
{
	var data = {
		Action:"SendAddFriendRequest",
		Peer:peer,
		Info:info
	};
	IMCore.SendCommand(
		function(ret)
		{
			callback(true);
		},
		function(ex)
		{
			callback(false, ex);
		},
		IMCore.Utility.RenderJson(data), "Kinfar.Frame.IMWeb Common_CH", false
	);
}

Module.AddFriend = function(callback, peer)
{
	var data = {
		Action:"AddFriend",
		Peer:peer
	};
	IMCore.SendCommand(
		function(ret)
		{
			callback(true);
		},
		function(ex)
		{
			callback(false, ex);
		},
		IMCore.Utility.RenderJson(data),"Kinfar.Frame.IMWeb Common_CH",false
	);
}

Module.AddToGroup = function(callback, user, group)
{
	var data = {
		Action: "AddToGroup",
		User: user,
		Group: group
	};
	
	IMCore.SendCommand(
		function(ret)
		{
			callback(true);
		},
		function(ex)
		{
			callback(false, ex);
		},
		IMCore.Utility.RenderJson(data),"Kinfar.Frame.IMWeb Common_CH",false
	);
}

Module.DeleteFriend = function(callback, peer)
{
	var data = {
		Action:"DeleteFriend",
		Peer:peer
	};
	IMCore.SendCommand(
		function(ret)
		{
			callback(true);
		},
		function(ex)
		{
			callback(false, ex);
		},
		IMCore.Utility.RenderJson(data),"Kinfar.Frame.IMWeb Common_CH",false
	);
}

Module.CreateGroup = function(callback, name, desc)
{
	var data = {
		Action:"CreateGroup",
		Name:name,
		Desc:desc
	};
	IMCore.SendCommand(
		function(ret)
		{
			callback(true);
		},
		function(ex)
		{
			callback(false, ex);
		},
		IMCore.Utility.RenderJson(data),"Kinfar.Frame.IMWeb Common_CH",false
	);
}

Module.GetAccountInfo = function(callback, name)
{
	var data = {
		Action:"GetAccountInfo",
		Name:name
	};
	IMCore.SendCommand(
		function(ret)
		{
			callback(true,ret.Info);
		},
		function(ex)
		{
			callback(false, ex);
		},
		IMCore.Utility.RenderJson(data),"Kinfar.Frame.IMWeb Common_CH",false
	);
}