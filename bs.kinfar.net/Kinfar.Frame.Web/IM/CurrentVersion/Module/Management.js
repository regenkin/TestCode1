IMCore.Link("StyleSheet", IMCore.GetUrl("Themes/Default/Management.css"), "text/css");

var Controls = null;
var Window = null, Control = null;

function init(completeCallback, errorCallback)
{
    function LoadModulesComplete()
    {
        Controls = IMCore.GetModule("Controls.js");

        Control = IMCore.GetModule("Controls.js").Control;

        _init(completeCallback, errorCallback);
    }

    IMCore.LoadModules(
        LoadModulesComplete, errorCallback,
        ["Controls.js"]
    );
}

function _init(completeCallback, errorCallback)
{
    try
    {
        //初始化代码，初始化完成后必须调用completeCallback;
        IMCore.LoadModules(function()
        {
            Common = IMCore.GetModule("Common.js");
            completeCallback();
        },
        errorCallback, ["Common.js"]);
    }
    catch (ex)
    {
        errorCallback(new IMCore.Exception(ex.mame, ex.message));
    }
}



function dispose(completeCallback, errorCallback)
{
    _dispose(completeCallback, errorCallback);
}

function _dispose(completeCallback, errorCallback)
{
    try
    {
        //卸载代码，卸载完成后必须调用completeCallback;
        completeCallback();
    }
    catch (ex)
    {
        errorCallback(new IMCore.Exception(ex.mame, ex.message));
    }
}

//共享全局变量和函数，在此定义的变量和函数将由该应用程序的所有实例共享
var Common = null;

function EmptyCallback()
{

}

function Match(reg, str)
{
    reg.lastIndex = 0;
    var ft = reg.exec(str);
    return (ft != null && ft.length == 1 && ft[0] == str)
}

function CheckEMail(email)
{
    return Match(/[a-zA-Z0-9._\-]+@[a-zA-Z0-9._\-]+/ig, email);
}

function CheckTel(tel)
{
    return Match(/[0-9\-]{6,30}/ig, tel);
}

function CheckMobile(mobile)
{
    return Match(/[0-9]{11,11}/ig, mobile);
}

function CheckName(name)
{
    return Match(/[0-9a-zA-Z]+/ig, name);
}

function CheckPassword(pwd)
{
    return Match(/[0-9a-zA-Z]{4,10}/ig, pwd);
}

function GetHeadIMG(info, size, gred)
{
    if (info.HeadIMG == "")
    {
        var url = IMCore.Config.ServiceUrl + "/images/HeadIMG/user"
        if (size > 0) url += "." + size;
        if (gred) url += ".gred";
        url += ".png";
        return url;
    }
    else
    {
        return String.format("{0}?user={1}&size={2}&gred={3}&headimg={4}", IMCore.GetPageUrl("headimg.aspx"), info.Name, size, gred, info.HeadIMG);
    }
}


Module.FriendPanel = FriendPanel;

function FriendPanelDS(config)
{
    var obj = this;
    if (config == undefined) config = {};
    if (config.ShowState == undefined) config.ShowState = false;

    obj.GetSubNodes = function(callback, node)
    {
        if (node == null)
        {
            var nodes = [{
                Name: "Users",
                Text: "联系人",
                Tag: null,
                ImageCss: "Image16_Folder"
            },
            {
                Name: "Groups",
                Text: "群组",
                Tag: null,
                ImageCss: "Image16_Folder"
            }];
            callback(nodes);
        }
        else
        {
            var name = node.GetName();
            if (name != "Users" && name != "Groups") return null;

            var type = (name == "Users" ? 0 : 1);

            Common.GetFriends(function(friends)
            {
                var nodes = [];
                for (var k in friends)
                {
                    var friend = friends[k];
                    if (friend.Type == type)
                    {
                        if (type == 0)
                        {
                            nodes.push({
                                Name: friend.Name.toUpperCase(),
                                Text: friend.Nickname,
                                Tag: friend,
                                HasChildren: false,
                                ImageSrc: GetHeadIMG(friend, 16, config.ShowState && friend.State == "Offline")
                            });
                        }
                        else
                        {
                            nodes.push({
                                Name: friend.Name.toUpperCase(),
                                Text: friend.Nickname,
                                Tag: friend,
                                HasChildren: false,
                                ImageCss: "Image16_Group"
                            });
                        }
                    }
                }
                callback(nodes);
            });
        }
    }

    return obj;
}

function FriendPanel(config)
{
    var This = this;
    var OwnerForm = this;
    
    config.Css = "FriendPanel";
    config.DataSource = new FriendPanelDS(config.DSConfig);
    
    var width = config.Width, height = config.Height;
    config.Width=243;
    config.Height=620;

    Controls.TreeView.call(This, config);

    var Base = {
        GetType: This.GetType,
        is: This.is
    };

    This.GetType = function() { return "FriendPanel"; }
    This.is = function(type) { return type == This.GetType() ? true : Base.is(type); }
    
    This.Resize(width,height);

    This.OnDblClick.Attach(
    function()
    {
        if (This.GetSelectedNode().GetTag() != null)
        {
            var form = IMCore.Session.GetGlobal("ChatService").Open(This.GetSelectedNode().GetTag().Name, false);
        }
    });

}


Module.FriendForm = FriendForm;

function FriendForm(config)
{
    var This = this;
    var OwnerForm = this;
    
    
    
    var width = config.Width, height = config.Height;
    config.Width=233;
    config.Height=707;

    Control.call(This, config);

    var Base = {
        GetType: This.GetType,
        is: This.is
    };

    This.GetType = function() { return "FriendForm"; }
    This.is = function(type) { return type == This.GetType() ? true : Base.is(type); }
    
    var m_Toolbar = new Controls.Toolbar({"Left":2,"Top":2,"Width":229,"Height":24,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":This,"Text":"功能菜单","Css":"toolbar","Items":[{"Text":"功能菜单","Css":"Image22_Add","Command":"Option"}]});
    
    
    
    m_Toolbar.OnCommand.Attach(
        function(command)
        {
            if (command == "Option")
            {
                var toolbar_btn_dom = m_Toolbar.GetControl(0);
                var coord = IMCore.Utility.GetClientCoord(toolbar_btn_dom);
                m_Menu.Popup(coord.X, coord.Y + toolbar_btn_dom.offsetHeight + 2);
            }
        }
    )

    
    var m_FriendPanel_Config={"Left":2,"Top":27,"Width":229,"Height":679,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top|Controls.AnchorStyle.Bottom,"Parent":This,"Text":"","Css":"control"};
    
            m_FriendPanel_Config.BorderWidth = 1;
            m_FriendPanel_Config.DSConfig = {
                ShowState: true
            };
    
    var m_FriendPanel = new FriendPanel(m_FriendPanel_Config);
    
    
    
    This.Resize(width,height);

    m_FriendPanel.Refresh(function()
    {
        m_FriendPanel.Expand(EmptyCallback, "/Users");
        m_FriendPanel.Expand(EmptyCallback, "/Groups");
    });
    
    var menuConfig = {
        Items: [{
            Text: "修改个人资料",
            ID: "UpdateSelfInfo"
        },
        {
            Text: "添加好友/群",
            ID: "AddFriend"
        },
        {
            Text: "好友/群管理",
            ID: "Management"
        },
        {
            Text: "消息管理",
            ID: "MsgHistory"
        }]
    };
    
    var m_Menu = new Controls.Menu(menuConfig);
    m_Menu.OnCommand.Attach(
    function(command)
    {
        if (command == "AddFriend")
        {
            IMCore.Session.GetGlobal("SingletonForm").ShowAddFriendForm();
        }
        else if (command == "Management")
        {
            IMCore.Session.GetGlobal("SingletonForm").ShowFriendManagementForm();
        }
        else if (command == "MsgHistory")
        {
            IMCore.Session.GetGlobal("SingletonForm").ShowMsgHistoryForm();
        }
        else if (command == "UpdateSelfInfo")
        {
            IMCore.Session.GetGlobal("SingletonForm").ShowUpdateSelfInfoForm();
        }
    });
    
    CurrentWindow.OnNotify.Attach(
    function(command, data)
    {
        if (command == "FriendInfoChanged")
        {
            m_FriendPanel.Refresh(function()
            {
                m_FriendPanel.Expand(EmptyCallback, "/Users");
                m_FriendPanel.Expand(EmptyCallback, "/Groups");
            });
        }
        else if (command == "UserInfoChanged")
        {
            CurrentWindow.SetTitle(IMCore.Session.GetUserInfo().Nickname);
        }
        else if (command == "UserStateChanged")
        {
            m_FriendPanel.GetAllNodes(function(node)
            {
                if (node.GetTag() != null && node.GetTag().ID == data.User)
                {
                    node.SetImage(GetHeadIMG(node.GetTag(), 16, data.State == "Offline"));
                }
            });
        }
    });

}


Module.AddFriendForm = AddFriendForm;

function AddFriendForm(config)
{
    var This = this;
    var OwnerForm = this;
    
    
    
    var width = config.Width, height = config.Height;
    config.Width=328;
    config.Height=248;

    Control.call(This, config);

    var Base = {
        GetType: This.GetType,
        is: This.is
    };

    This.GetType = function() { return "AddFriendForm"; }
    This.is = function(type) { return type == This.GetType() ? true : Base.is(type); }
    
    var m_Tab = new Controls.SimpleTabControl({"Left":1,"Top":1,"Width":326,"Height":246,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top|Controls.AnchorStyle.Bottom,"Parent":This,"Text":"","Css":"simple_tab_gred","Tabs":[{"Text":"添加好友/群","Width":120,"ID":"ID100000007","IsSelected":true}],"BorderWidth":1});
    
    
    
    m_Tab.OnSelectedTab.Attach(
        function(index,preIndex)
        {
            
        }
    )

    
    var label2 = new Controls.Label({"Left":12,"Top":20,"Width":95,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":m_Tab.GetPanel(0),"Text":"群/用户账户名：","Css":"label"});
    
    

    
    var m_EditNameF = new Controls.TextBox({"Left":112,"Top":16,"Width":202,"Height":22,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":m_Tab.GetPanel(0),"Text":"","Css":"textbox","BorderWidth":1});
    
    if (IMCore.Params["Name"] != undefined) m_EditNameF.SetText(IMCore.Params["Name"]);

    
    var label4 = new Controls.Label({"Left":11,"Top":54,"Width":139,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":m_Tab.GetPanel(0),"Text":"验证信息：","Css":"label"});
    
    

    
    var m_EditInfoF = new Controls.TextArea({"Left":11,"Top":74,"Width":302,"Height":102,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top|Controls.AnchorStyle.Bottom,"Parent":m_Tab.GetPanel(0),"Text":"","Css":"textarea","BorderWidth":1});
    
    

    
    var m_BtnAddFriend = new Controls.Button({"Left":249,"Top":184,"Width":64,"Height":26,"AnchorStyle":Controls.AnchorStyle.Right|Controls.AnchorStyle.Bottom,"Parent":m_Tab.GetPanel(0),"Text":"确定","Css":"button_default"});
    
    
    
    m_BtnAddFriend.OnClick.Attach(
        function(btn)
        {
            Common.SendAddFriendRequest(
            function(result, ex)
            {
                if (!result)
                {
                    IMCore.Utility.ShowError(ex.toString());
                }
                else
                {
                    m_EditNameF.SetText("");
                    m_EditInfoF.SetText("")
                    IMCore.Utility.ShowFloatForm("添加好友的请求已发送，等待对方确认...", "text");
                }
            },
            m_EditNameF.GetText(), m_EditInfoF.GetText());
        }
    )
    
    This.Resize(width,height);

    

}


Module.FriendManagementForm = FriendManagementForm;

function FriendManagementForm(config)
{
    var This = this;
    var OwnerForm = this;
    
    
    
    var width = config.Width, height = config.Height;
    config.Width=596;
    config.Height=502;

    Control.call(This, config);

    var Base = {
        GetType: This.GetType,
        is: This.is
    };

    This.GetType = function() { return "FriendManagementForm"; }
    This.is = function(type) { return type == This.GetType() ? true : Base.is(type); }
    
    var tab2 = new Controls.SimpleTabControl({"Left":1,"Top":1,"Width":594,"Height":500,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top|Controls.AnchorStyle.Bottom,"Parent":This,"Text":"","Css":"simple_tab_gred","Tabs":[{"Text":"好友管理","Width":80,"ID":"ID100000010","IsSelected":true},{"Text":"群管理","Width":80,"ID":"ID100000011","IsSelected":false},{"Text":"所有用户","Width":80,"ID":"ID100000012","IsSelected":false},{"Text":"所有群组","Width":80,"ID":"ID100000013","IsSelected":false}],"BorderWidth":1});
    
    
    
    tab2.OnSelectedTab.Attach(
        function(index,preIndex)
        {
            if (index == 0)
            {
                CurrentWindow.Waiting("正在载入好友列表...");
                m_FrameFriend.Navigate(String.format("FriendList.aspx?random={0}", (new Date()).getTime()));
            }
            else if (index == 1)
            {
                CurrentWindow.Waiting("正在载入群列表...");
                m_FrameGroup.Navigate(String.format("GroupList.aspx?random={0}", (new Date()).getTime()));
            
            }
            else if (index == 2)
            {
                CurrentWindow.Waiting("正在载入用户列表...");
                m_FrameAllUsers.Navigate(String.format("AllUSers.aspx?random={0}", (new Date()).getTime()));
            
            }
            else if (index == 3)
            {
                CurrentWindow.Waiting("正在载入群列表...");
                m_FrameAllGroups.Navigate(String.format("AllGroups.aspx?random={0}", (new Date()).getTime()));
            
            }
        }
    )

    
    
    var m_FrameFriend_Config={"Left":6,"Top":6,"Width":580,"Height":462,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top|Controls.AnchorStyle.Bottom,"Parent":tab2.GetPanel(0),"Text":"","Css":"frame"};
    
    m_FrameFriend_Config.BorderWidth = 1;
    
    var m_FrameFriend = new Controls.Frame(m_FrameFriend_Config);
    
    
    
    m_FrameFriend.OnResized.Attach(
        function(btn)
        {
            
        }
    )

    
    
    var m_FrameGroup_Config={"Left":6,"Top":6,"Width":580,"Height":462,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top|Controls.AnchorStyle.Bottom,"Parent":tab2.GetPanel(1),"Text":"","Css":"frame"};
    
    m_FrameGroup_Config.BorderWidth = 1;
    
    var m_FrameGroup = new Controls.Frame(m_FrameGroup_Config);
    
    
    
    m_FrameGroup.OnResized.Attach(
        function(btn)
        {
            
        }
    )

    
    
    var m_FrameAllUsers_Config={"Left":6,"Top":6,"Width":580,"Height":462,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top|Controls.AnchorStyle.Bottom,"Parent":tab2.GetPanel(2),"Text":"","Css":"frame"};
    
    m_FrameAllUsers_Config.BorderWidth = 1;
    
    var m_FrameAllUsers = new Controls.Frame(m_FrameAllUsers_Config);
    
    
    
    m_FrameAllUsers.OnResized.Attach(
        function(btn)
        {
            
        }
    )

    
    
    var m_FrameAllGroups_Config={"Left":6,"Top":6,"Width":580,"Height":462,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top|Controls.AnchorStyle.Bottom,"Parent":tab2.GetPanel(3),"Text":"","Css":"frame"};
    
    m_FrameAllGroups_Config.BorderWidth = 1;
    
    var m_FrameAllGroups = new Controls.Frame(m_FrameAllGroups_Config);
    
    
    
    m_FrameAllGroups.OnResized.Attach(
        function(btn)
        {
            
        }
    )
    
    This.Resize(width,height);

    tab2.Select(0);
    
    tab2.SetTabVisible(2, IMCore.Session.GetUserName().toLowerCase() == "sa");
    tab2.SetTabVisible(3, IMCore.Session.GetUserName().toLowerCase() == "sa");
    
    CurrentWindow.OnNotify.Attach(
    function(command, data)
    {
        if (command == "FriendInfoChanged")
        {
            if (tab2.GetSelectedIndex() == 0)
            {
                CurrentWindow.Waiting("正在载入好友列表...");
                m_FrameFriend.Navigate(String.format("FriendList.aspx?random={0}", (new Date()).getTime()));
            }
            else if (tab2.GetSelectedIndex() == 1)
            {
                CurrentWindow.Waiting("正在载入群列表...");
                m_FrameGroup.Navigate(String.format("GroupList.aspx?Type=1&random={0}", (new Date()).getTime()));
            }
        }
    });

}


Module.ImageControl = ImageControl;

function ImageControl(config)
{
    var This = this;
    var OwnerForm = this;
    
    config.Css = "ImageControl";
    
    var width = config.Width, height = config.Height;
    config.Width=200;
    config.Height=200;

    Control.call(This, config);

    var Base = {
        GetType: This.GetType,
        is: This.is
    };

    This.GetType = function() { return "ImageControl"; }
    This.is = function(type) { return type == This.GetType() ? true : Base.is(type); }
    
    This.Resize(width,height);

    var img = null;
    
    function centerImage()
    {
        if (img.width > config.ImageWidth || img.height > config.ImageHeight)
        {
            if (img.width * config.ImageHeight > img.height * config.ImageWidth)
            {
                img.height = config.ImageWidth * img.height / img.width;
                img.width = config.ImageWidth;
            }
            else
            {
                img.width = config.ImageHeight * img.width / img.height;
                img.height = config.ImageHeight;
            }
        }
        img.style.marginLeft = (This.GetClientWidth() - img.width) / 2 + 'px';
        img.style.marginTop = (This.GetClientHeight() - img.height) / 2 + 'px';
    }
    
    This.OnResized.Attach(
    function()
    {
        if (img != null)
        {
            img.style.marginLeft = (This.GetClientWidth() - img.width) / 2 + 'px';
            img.style.marginTop = (This.GetClientHeight() - img.height) / 2 + 'px';
        }
    });
    
    This.LoadImage = function(src)
    {
        This.GetDom().innerHTML = "<img/>";
        img = This.GetDom().firstChild;
        img.onload = centerImage;
        img.src = src;
    }
    
    This.GetImageSrc = function()
    {
        return img == null ? "": img.src;
    }

}




function UserInfoPanel(config)
{
    var This = this;
    var OwnerForm = this;
    
    config.Css="UpdateSelfInfo";
    
    var width = config.Width, height = config.Height;
    config.Width=380;
    config.Height=383;

    Control.call(This, config);

    var Base = {
        GetType: This.GetType,
        is: This.is
    };

    This.GetType = function() { return "UserInfoPanel"; }
    This.is = function(type) { return type == This.GetType() ? true : Base.is(type); }
    
    var label2 = new Controls.Label({"Left":11,"Top":4,"Width":51,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":This,"Text":"登录名：","Css":"label"});
    
    

    
    var m_EditName = new Controls.TextBox({"Left":64,"Top":0,"Width":316,"Height":22,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":This,"Text":"","Css":"textbox","BorderWidth":1});
    
    m_EditName.GetTextBoxDom().readOnly = true;

    
    var m_EditNickname = new Controls.TextBox({"Left":64,"Top":30,"Width":316,"Height":22,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":This,"Text":"","Css":"textbox","BorderWidth":1});
    
    

    
    var label5 = new Controls.Label({"Left":23,"Top":34,"Width":42,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":This,"Text":"昵称：","Css":"label"});
    
    

    
    var m_EditEMail = new Controls.TextBox({"Left":64,"Top":60,"Width":316,"Height":22,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":This,"Text":"","Css":"textbox","BorderWidth":1});
    
    

    
    var label7 = new Controls.Label({"Left":24,"Top":64,"Width":40,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":This,"Text":"邮箱：","Css":"label"});
    
    

    
    var label8 = new Controls.Label({"Left":24,"Top":94,"Width":39,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":This,"Text":"主页：","Css":"label"});
    
    

    
    var m_EditHomePage = new Controls.TextBox({"Left":64,"Top":90,"Width":316,"Height":22,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":This,"Text":"","Css":"textbox","BorderWidth":1});
    
    

    
    var m_HeadIMG_Config={"Left":64,"Top":184,"Width":316,"Height":180,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":This,"Text":"","Css":"control"};
    
            m_HeadIMG_Config.BorderWidth = 1;
            m_HeadIMG_Config.ImageWidth = 150;
            m_HeadIMG_Config.ImageHeight = 150;
    
    var m_HeadIMG = new ImageControl(m_HeadIMG_Config);
    
    

    
    var label11 = new Controls.Label({"Left":20,"Top":186,"Width":42,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":This,"Text":"头像：","Css":"label"});
    
    

    
    var m_EditRemark = new Controls.TextArea({"Left":64,"Top":120,"Width":316,"Height":58,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":This,"Text":"","Css":"textarea","BorderWidth":1});
    
    

    
    var label14 = new Controls.Label({"Left":0,"Top":124,"Width":64,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":This,"Text":"个性签名：","Css":"label"});
    
    

    
    var label15 = new Controls.Label({"Left":64,"Top":368,"Width":117,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":This,"Text":"","Css":"label"});
    
    label15.GetDom().innerHTML = "<a class=''>修改头像...</a><input type='file' name='file_headimg' id='file_headimg'/>"
    label15.GetDom().childNodes[1].onchange = function()
    {
        if (ClientMode)
        {
            var file = label15.GetDom().childNodes[1];
            if (file.value.indexOf("fakepath") >= 0)
            {
                file.select();
                m_HeadIMG.LoadImage(document.selection.createRange().text);
            }
            else
            {
                m_HeadIMG.LoadImage(file.value);
            }
        }
        else
        {
            m_HeadIMG.LoadImage(String.format("{0}?FileName=/{1}/Public/Images/HeadImg/head_changed_warning.png", IMCore.GetPageUrl("download.aspx"), IMCore.Session.GetUserName()));
        }
    }
    
    This.Resize(width,height);

    This.SetInfo = function(info)
    {
        m_EditName.SetText(info.Name);
        m_EditNickname.SetText(info.Nickname);
        m_EditEMail.SetText(info.EMail);
        m_EditHomePage.SetText(info.HomePage);
        m_EditRemark.SetText(info.Remark);
        m_HeadIMG.LoadImage(String.format("{0}?user={1}&gred=false&headimg={2}", IMCore.GetPageUrl("headimg.aspx"), info.Name, info.HeadIMG));
    }
    
    This.GetInfo = function()
    {
        if (m_EditName.GetText() == "")
        {
            IMCore.Utility.ShowWarning("登录名不能为空！");
            m_EditName.GetTextBoxDom().focus();
            return null;
        }
    
        if (m_EditName.GetText().length > 10 || m_EditName.GetText().length < 2)
        {
            IMCore.Utility.ShowWarning("登录名格式错误，登录名只能包含数字，英文字母，长度为2-10个字符！");
            m_EditName.GetTextBoxDom().focus();
            return null;
        }
    
        if (!CheckName(m_EditName.GetText()))
        {
            IMCore.Utility.ShowWarning("登录名格式错误，登录名只能包含数字，英文字母，长度为2-10个字符！");
            m_EditName.GetTextBoxDom().focus();
            return null;
        }
    
        if (m_EditNickname.GetText() == "")
        {
            IMCore.Utility.ShowWarning("昵称不能为空！");
            m_EditNickname.GetTextBoxDom().focus();
            return null;
        }
    
        if (m_EditNickname.GetText().length > 16 || m_EditNickname.GetText().length < 2)
        {
            IMCore.Utility.ShowWarning("姓名格式错误，长度为2-16个字符");
            m_EditNickname.GetTextBoxDom().focus();
            return;
        }
    
        if (m_EditEMail.GetText() == "")
        {
            IMCore.Utility.ShowWarning("邮箱不能为空！");
            m_EditEMail.GetTextBoxDom().focus();
            return null;
        }
    
        if (m_EditEMail.GetText().length > 80)
        {
            IMCore.Utility.ShowWarning("邮箱不能超过80字符！");
            m_EditEMail.GetTextBoxDom().focus();
            return null;
        }
    
        if (!CheckEMail(m_EditEMail.GetText()))
        {
            IMCore.Utility.ShowWarning("邮箱格式错误！");
            m_EditEMail.GetTextBoxDom().focus();
            return null;
        }
    
        var info = {
            Name: m_EditName.GetText(),
            Nickname: m_EditNickname.GetText(),
            EMail: m_EditEMail.GetText(),
            HomePage: m_EditHomePage.GetText(),
            Remark: m_EditRemark.GetText()
        };
    
        return info;
    }

}




function GroupInfoPanel(config)
{
    var This = this;
    var OwnerForm = this;
    
    config.Css="UpdateSelfInfo";
    
    var width = config.Width, height = config.Height;
    config.Width=324;
    config.Height=388;

    Control.call(This, config);

    var Base = {
        GetType: This.GetType,
        is: This.is
    };

    This.GetType = function() { return "GroupInfoPanel"; }
    This.is = function(type) { return type == This.GetType() ? true : Base.is(type); }
    
    var label2 = new Controls.Label({"Left":10,"Top":4,"Width":52,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":This,"Text":"群名称：","Css":"label"});
    
    

    
    var m_EditName = new Controls.TextBox({"Left":64,"Top":0,"Width":260,"Height":22,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":This,"Text":"","Css":"textbox","BorderWidth":1});
    
    m_EditName.GetTextBoxDom().readOnly = true;

    
    var m_EditNickname = new Controls.TextBox({"Left":64,"Top":30,"Width":260,"Height":22,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":This,"Text":"","Css":"textbox","BorderWidth":1});
    
    

    
    var label5 = new Controls.Label({"Left":10,"Top":34,"Width":55,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":This,"Text":"群简称：","Css":"label"});
    
    

    
    var label8 = new Controls.Label({"Left":10,"Top":64,"Width":53,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":This,"Text":"群主页：","Css":"label"});
    
    

    
    var m_EditHomePage = new Controls.TextBox({"Left":64,"Top":60,"Width":260,"Height":22,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":This,"Text":"","Css":"textbox","BorderWidth":1});
    
    

    
    var m_HeadIMG_Config={"Left":64,"Top":184,"Width":260,"Height":180,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":This,"Text":"","Css":"control"};
    
            m_HeadIMG_Config.BorderWidth = 1;
            m_HeadIMG_Config.ImageWidth = 150;
            m_HeadIMG_Config.ImageHeight = 150;
    
    var m_HeadIMG = new ImageControl(m_HeadIMG_Config);
    
    

    
    var label11 = new Controls.Label({"Left":9,"Top":186,"Width":53,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":This,"Text":"群LOGO：","Css":"label"});
    
    

    
    var m_EditRemark = new Controls.TextArea({"Left":64,"Top":92,"Width":260,"Height":86,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":This,"Text":"","Css":"textarea","BorderWidth":1});
    
    

    
    var label14 = new Controls.Label({"Left":9,"Top":94,"Width":55,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":This,"Text":"群介绍：","Css":"label"});
    
    

    
    var label15 = new Controls.Label({"Left":64,"Top":368,"Width":117,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":This,"Text":"","Css":"label"});
    
    label15.GetDom().innerHTML = "<a class=''>修改头像...</a><input type='file' name='file_headimg' id='file_headimg'/>"
    label15.GetDom().childNodes[1].onchange = function()
    {
        if (ClientMode)
        {
            var file = label15.GetDom().childNodes[1];
            if (file.value.indexOf("fakepath") >= 0)
            {
                file.select();
                m_HeadIMG.LoadImage(document.selection.createRange().text);
            }
            else
            {
                m_HeadIMG.LoadImage(file.value);
            }
        }
        else
        {
            m_HeadIMG.LoadImage(String.format("{0}?FileName=/{1}/Public/Images/HeadImg/head_changed_warning.png", IMCore.GetPageUrl("download.aspx"), IMCore.Session.GetUserName()));
        }
    }
    
    This.Resize(width,height);

    This.SetInfo = function(info)
    {
        m_EditName.SetText(info.Name);
        m_EditNickname.SetText(info.Nickname);
        m_EditHomePage.SetText(info.HomePage);
        m_EditRemark.SetText(info.Remark);
        m_HeadIMG.LoadImage(String.format("{0}?user={1}&gred=false&headimg={2}", IMCore.GetPageUrl("headimg.aspx"), info.Name, info.HeadIMG));
    }
    
    This.GetInfo = function()
    {
        if (m_EditName.GetText() == "")
        {
            IMCore.Utility.ShowWarning("登录名不能为空！");
            m_EditName.GetTextBoxDom().focus();
            return null;
        }
    
        if (m_EditName.GetText().length > 10 || m_EditName.GetText().length < 2)
        {
            IMCore.Utility.ShowWarning("登录名格式错误，登录名只能包含数字，英文字母，长度为2-10个字符！");
            m_EditName.GetTextBoxDom().focus();
            return null;
        }
    
        if (!CheckName(m_EditName.GetText()))
        {
            IMCore.Utility.ShowWarning("登录名格式错误，登录名只能包含数字，英文字母，长度为2-10个字符！");
            m_EditName.GetTextBoxDom().focus();
            return null;
        }
    
        if (m_EditNickname.GetText() == "")
        {
            IMCore.Utility.ShowWarning("昵称不能为空！");
            m_EditNickname.GetTextBoxDom().focus();
            return null;
        }
    
        if (m_EditNickname.GetText().length > 16 || m_EditNickname.GetText().length < 2)
        {
            IMCore.Utility.ShowWarning("姓名格式错误，长度为2-16个字符");
            m_EditNickname.GetTextBoxDom().focus();
            return;
        }
    
        var info = {
            Name: m_EditName.GetText(),
            Nickname: m_EditNickname.GetText(),
            HomePage: m_EditHomePage.GetText(),
            Remark: m_EditRemark.GetText()
        };
    
        return info;
    }

}


Module.UpdateSelfInfo = UpdateSelfInfo;

function UpdateSelfInfo(config)
{
    var This = this;
    var OwnerForm = this;
    
    
    
    var width = config.Width, height = config.Height;
    config.Width=360;
    config.Height=475;

    Control.call(This, config);

    var Base = {
        GetType: This.GetType,
        is: This.is
    };

    This.GetType = function() { return "UpdateSelfInfo"; }
    This.is = function(type) { return type == This.GetType() ? true : Base.is(type); }
    
    var tab1 = new Controls.SimpleTabControl({ "Left": 1, "Top": 1, "Width": 358, "Height": 473, "AnchorStyle": Controls.AnchorStyle.Left | Controls.AnchorStyle.Right | Controls.AnchorStyle.Top | Controls.AnchorStyle.Bottom, "Parent": This, "Text": "", "Css": "simple_tab_gred", "Tabs": [{ "Text": "个人资料", "Width": 80, "ID": "ID100000016", "IsSelected": true },{"Text":"修改密码","Width":80,"ID":"ID100000017","IsSelected":false}], "BorderWidth": 1 });
    
    tab1.Select(GetState().Tab);
    
    tab1.OnSelectedTab.Attach(
        function (index, preIndex) {
            var state = GetState();
            state.Tab = index;
            ResetState(state);
        }
    );
    
    var m_Info_Config={"Left":10,"Top":10,"Width":337,"Height":398,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top|Controls.AnchorStyle.Bottom,"Parent":tab1.GetPanel(0),"Text":"","Css":"control"};
    
    var m_Info = new UserInfoPanel(m_Info_Config);
    
    var m_BtnOK = new Controls.Button({"Left":276,"Top":414,"Width":70,"Height":26,"AnchorStyle":Controls.AnchorStyle.Right|Controls.AnchorStyle.Bottom,"Parent":tab1.GetPanel(0),"Text":"更 新","Css":"button_default"});
    
    m_BtnOK.OnClick.Attach(
        function (btn) {
            var info = m_Info.GetInfo();
            if (info != null) {
                CurrentWindow.Waiting("正在更新个人资料...");
                DoCommand("Update", info);
            }
        }
    );
    var label4 = new Controls.Label({"Left":22,"Top":27,"Width":52,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":tab1.GetPanel(1),"Text":"原密码：","Css":"label"});
    
    var label8 = new Controls.Label({"Left":22,"Top":65,"Width":52,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":tab1.GetPanel(1),"Text":"新密码：","Css":"label"});
    
    var m_EditPrePassword = new Controls.PasswordBox({"Left":77,"Top":23,"Width":265,"Height":22,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":tab1.GetPanel(1),"Text":"","Css":"textbox","BorderWidth":1});
    
    var m_EditPassword = new Controls.PasswordBox({"Left":77,"Top":61,"Width":265,"Height":22,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":tab1.GetPanel(1),"Text":"","Css":"textbox","BorderWidth":1});
    
    var m_EditPasswordConfirm = new Controls.PasswordBox({"Left":77,"Top":98,"Width":265,"Height":22,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":tab1.GetPanel(1),"Text":"","Css":"textbox","BorderWidth":1});
    
    var label12 = new Controls.Label({"Left":10,"Top":102,"Width":64,"Height":14,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":tab1.GetPanel(1),"Text":"密码确认：","Css":"label"});
    
    var BtnAlterPassword = new Controls.Button({"Left":251,"Top":142,"Width":90,"Height":26,"AnchorStyle":Controls.AnchorStyle.Right|Controls.AnchorStyle.Top,"Parent":tab1.GetPanel(1),"Text":"修改密码","Css":"button_default"});
    
    BtnAlterPassword.OnClick.Attach(
        function (btn) {
            /*if (m_EditPrePassword.GetText() == "")
            {
                IMCore.Utility.ShowWarning("请输入原密码！");
                m_EditPrePassword.GetPasswordDom().focus();
                return;
            }
            
            if (!CheckPassword(m_EditPrePassword.GetText()))
            {
                IMCore.Utility.ShowWarning("密码必须由4-10位字母或数字组成！");
                m_EditPrePassword.GetPasswordDom().focus();
                return;
            }
            */
            if (m_EditPassword.GetText() == "") {
                IMCore.Utility.ShowWarning("请输入新密码！");
                m_EditPassword.GetPasswordDom().focus();
                return;
            }

            if (!CheckPassword(m_EditPassword.GetText())) {
                IMCore.Utility.ShowWarning("密码必须由4-10位字母或数字组成！");
                m_EditPassword.GetPasswordDom().focus();
                return;
            }

            if (m_EditPasswordConfirm.GetText() != m_EditPassword.GetText()) {
                IMCore.Utility.ShowWarning("两次输入密码不一致！");
                m_EditPasswordConfirm.GetPasswordDom().focus();
                return;
            }

            var data = {
                PreviousPassword: m_EditPrePassword.GetText(),
                Password: m_EditPassword.GetText()
            };

            CurrentWindow.Waiting("正在修改密码...");
            DoCommand("ChangePassword", data);
        }
    );
    
    This.Resize(width,height);

    m_Info.SetInfo(GetState().SelfInfo);
    
    tab1.SetTabVisible(1, false);
}


Module.UpdateGroupInfo = UpdateGroupInfo;

function UpdateGroupInfo(config)
{
    var This = this;
    var OwnerForm = this;
    
    
    
    var width = config.Width, height = config.Height;
    config.Width=360;
    config.Height=475;

    Control.call(This, config);

    var Base = {
        GetType: This.GetType,
        is: This.is
    };

    This.GetType = function() { return "UpdateGroupInfo"; }
    This.is = function(type) { return type == This.GetType() ? true : Base.is(type); }
    
    var tab1 = new Controls.SimpleTabControl({"Left":1,"Top":1,"Width":358,"Height":473,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Right|Controls.AnchorStyle.Top|Controls.AnchorStyle.Bottom,"Parent":This,"Text":"","Css":"simple_tab_gred","Tabs":[{"Text":"群资料","Width":80,"ID":"ID100000020","IsSelected":true}],"BorderWidth":1});
    
    
    
    tab1.OnSelectedTab.Attach(
        function(index,preIndex)
        {
            
        }
    )

    
    var m_Info_Config={"Left":10,"Top":10,"Width":337,"Height":398,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Top,"Parent":tab1.GetPanel(0),"Text":"","Css":"control"};
    
            
    
    var m_Info = new GroupInfoPanel(m_Info_Config);
    
    

    
    var BtnOK = new Controls.Button({"Left":265,"Top":415,"Width":81,"Height":26,"AnchorStyle":Controls.AnchorStyle.Left|Controls.AnchorStyle.Bottom,"Parent":tab1.GetPanel(0),"Text":"更 新","Css":"button_default"});
    
    
    
    BtnOK.OnClick.Attach(
        function(btn)
        {
            var info = m_Info.GetInfo();
            if (info != null)
            {
                CurrentWindow.Waiting("正在更新群资料...");
                DoCommand("Update", info);
            }
        }
    )
    
    This.Resize(width,height);

    m_Info.SetInfo(GetState().AccountInfo);

}

