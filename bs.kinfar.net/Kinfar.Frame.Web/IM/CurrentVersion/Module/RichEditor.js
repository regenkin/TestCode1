

var Control=null;
var Controls=null;

var PageTemp=
	"<html>\r\n"+
		"<head>\r\n"+
		"</head>\r\n"+
		"<body>\r\n"+
		"</body>\r\n"+
	"</html>\r\n";

var Fonts=[
	"宋体",
	"雅黑",
	"Arial",
	"Courier New",
	"Times New Roman",
	"Verdana"
];

function init(completeCallback,errorCallback)
{
	IMCore.LoadModules(
		function()
		{			
			Control=IMCore.GetModule("Controls.js").Control;
			
			Controls=IMCore.GetModule("Controls.js");
			
			completeCallback();
		},
		errorCallback,
		["Controls.js"]
	);
}

function SetNodeStyle(doc,node,name,value)
{
	if(IMCore.Utility.IsTextNode(node))
	{
		return node;
	}
	else
	{
		node.style[name]=value;
		
		for(var i=0;i<node.childNodes.length;i++)
		{
			var cn=node.childNodes[i];
			if(!IMCore.Utility.IsTextNode(node))
			{
				SetNodeStyle(doc,cn,name,value);
			}
		}
	
		return node;
	}
}

function SetStyle(doc,html,name,value)
{
	var dom=doc.createElement("DIV");
	dom.innerHTML=html;
		
	for(var i=0;i<dom.childNodes.length;i++)
	{
		var node=dom.childNodes[i];
		
		if(IMCore.Utility.IsTextNode(node))
		{
			var span=doc.createElement("SPAN");
			span.style[name] = value;
			if (node.nodeValue != undefined) span.innerHTML = node.nodeValue.replace(/\</ig, function() { return "&lt;"; });
			else if (node.textContent != undefined) span.innetHTML = node.textContent.replace(/\</ig, function() { return "&lt;"; });
			dom.replaceChild(span,node);
		}
		else
		{
			SetNodeStyle(doc,node,name,value);
		}
	}
	
	return dom.innerHTML;
}

var EmotionForm = (function()
{
	var obj = {};
	
	var dom = document.createElement("DIV");
	dom.className = "EmotionForm";
	dom.id = IMCore.GenerateUniqueId();
	dom.style.display = "none";
	dom.style.width = "436px";
	dom.style.height = "175px";

	for(var y = 0; y < 6; y++)
	{
		for(var x = 0; x < 15; x++)
		{
			(function(x,y)
			{
				var emot_div = document.createElement("DIV");
				emot_div.className = "EmotUnit";
				IMCore.Utility.AttachButtonEvent(emot_div, "EmotUnit", "EmotUnit_hover", "EmotUnit_hover");
				emot_div.onmousedown = function(evt)
				{
					IMCore.Utility.CancelBubble(evt == undefined ? window.event : evt);
					obj.Close();
					if(_callback != null) _callback(String.format("Public/Images/Emotion/e{0}.gif",y*15+x+100));
				}
				dom.appendChild(emot_div);
			})(x,y)
		}
	}
	
	var _callback = null;
	
	obj.Popup = function(x,y,callback)
	{		
		if(dom.style.display == "none")
		{
			Desktop.EnterMove("default");
			
			dom.style.left = Math.max(x, 0) + "px";
			dom.style.top = Math.max(y, 0) + "px";
			dom.style.display = "block";
			
			_callback = callback;
		}
	}
	
	obj.Close = function()
	{
		if(dom.style.display == "block")
		{
			dom.style.display = "none";
			Desktop.LeaveMove();
		}
	}
	
	obj.GetDom = function()
	{
		return dom;
	}
	
	obj.GetWidth = function()
	{
		return 436;
	}
	
	obj.GetHeight = function()
	{
		return 175;
	}
	
	document.body.appendChild(dom);
	
	IMCore.Utility.AttachEvent(
		document, "mousedown",
		function()
		{
			obj.Close();
		}
	);
	
	return obj;
})();



/*
config={
	...：继承Controls
}
*/

function RichEditor(config)
{
    var This=this;

    Control.call(This,config);
    
    var Base={
        GetType:This.GetType,
        is:This.is
    }
    
    This.is=function(type){return type==This.GetType()?true:Base.is(type);}
    This.GetType=function(){return "RichEditor";}
    
	var m_Toolbar=new Controls.Toolbar(
		{
			Left:0,Top:0,Width:This.GetClientWidth(),Height:24,
			BorderWidth:0,Css:'toolbar',
			Parent:This,
			AnchorStyle:Controls.AnchorStyle.Left | Controls.AnchorStyle.Right | Controls.AnchorStyle.Top,
			Text:"",
            Items:[
				{Css:"Image22_B",Text:"",Command:"B"},
				{Css:"Image22_I",Text:"",Command:"I"},
				{Css:"Image22_U",Text:"",Command:"U"},
				{Type:"DropDownList",Width:120},
				{Type:"DropDownList",Width:50},
				{Css:"Image22_Emotion",Text:"表情",Command:"AddEmotion"},
				{Css:"Image22_Clear",Text:"清除格式",Command:"Clear"},
				{Css:"Image22_Delete",Text:"清空",Command:"Empty"}
			]
		}
	);
	
	for(var i in Fonts)
	{
		m_Toolbar.GetControl(3).AddItem(Fonts[i]);
	}
	m_Toolbar.GetControl(3).SetValue(Fonts[0]);
	IMCore.Utility.DisableSelect(m_Toolbar.GetControl(3).GetDom(),true);
	IMCore.Utility.DisableSelect(m_Toolbar.GetControl(3).GetListDom(),true);
	
	m_Toolbar.GetControl(3).OnChanged.Attach(
		function()
		{
			m_EditorDoc.execCommand("FontName",false,m_Toolbar.GetControl(3).GetValue());
		}
	);
	
	for (var i = 12; i <= 24; i += 2)
	{
		m_Toolbar.GetControl(4).AddItem(i);
	}
	for (var i = 36; i <= 72; i += 12)
	{
		m_Toolbar.GetControl(4).AddItem(i);
	}
	m_Toolbar.GetControl(4).SetValue(12);
	IMCore.Utility.DisableSelect(m_Toolbar.GetControl(4).GetDom(),true);
	IMCore.Utility.DisableSelect(m_Toolbar.GetControl(4).GetListDom(),true);
	
	m_Toolbar.GetControl(4).OnChanged.Attach(
		function()
		{
			var html=This.GetSelectionHtml();
			This.SaveSelection();
			This.ReplaceSaveSelection(SetStyle(m_EditorDoc,html,"fontSize",m_Toolbar.GetControl(4).GetText()+"px"));
		}
	);
	
	var m_LastImagePath = "";
	
	m_Toolbar.OnCommand.Attach(
		function(command)
		{
			switch(command)
			{
			case "B":
				{
					m_EditorDoc.execCommand("Bold",false,null);
					m_EditorWindow.focus();
					break;
				}
			case "I":
				{
					m_EditorDoc.execCommand("Italic",false,null);
					m_EditorWindow.focus();
					break;
				}
			case "U":
				{
					m_EditorDoc.execCommand("Underline",false,null);
					m_EditorWindow.focus();
					break;
				}
			case "A":
				{
					m_EditorDoc.execCommand("CreateLink",false);
					m_EditorWindow.focus();
					break;
				}
			case "Clear":
				{
					var html=This.GetSelectionHtml();
					This.SaveSelection();
					var temp=m_EditorDoc.createElement("DIV");
					temp.innerHTML=html;
					This.ReplaceSaveSelection(IMCore.Utility.ClearHtml(temp));
					break;
				}
			case "Empty":
				{
					This.SetValue("");
					break;
				}
			case "AddEmotion":
				{
					var btnRect = IMCore.Utility.GetClientCoord(m_Toolbar.GetControl(5));
					var bodyRect = IMCore.Utility.GetClientCoord(document.body);
					var y = 0;
					if(btnRect.Y - bodyRect.Y > EmotionForm.GetHeight()) y = btnRect.Y - bodyRect.Y - EmotionForm.GetHeight();
					else y = btnRect.Y + m_Toolbar.GetControl(5).offsetHeight;
					
					This.SaveSelection();
					
					EmotionForm.Popup(
						btnRect.X - bodyRect.X - 300 + m_Toolbar.GetControl(5).offsetWidth,
						y,
						function(path)
						{
							var imgHTML=String.format(
								"<img src='download.aspx?FileName={0}'/>",
								escape(path)
							);
							if(!This.ReplaceSaveSelection(imgHTML))
							{
								This.Append(imgHTML);
							}
						}
					);
					break;
				}
			}
		}
	);
	
	This.CreateFileHtml=function(paths)
	{
		var ret="";
		for(var i in paths)
		{
			var aHTML=String.format(
				"<a href='download.los?FileName={{Accessory {0}}' target='_blank'>{1}</a>",
				escape(String.format("src='{0}'",paths[i])),
				IMCore.IO.Path.GetFileName(escape(paths[i]))
			);
			ret += aHTML;
		}
		return ret;
	}
	
    var m_Editor=new Editor();
    var m_Frame=m_Editor.GetFrame();
    var m_EditorDoc=m_Editor.GetFrame().contentWindow.document;
    var m_EditorWindow=m_Editor.GetFrame().contentWindow;
    
    if(IMCore.GetBrowser()=="Firefox")
    {
		m_Frame.onload=function()
		{
			m_EditorDoc.designMode="on";
		}
	}
	else
	{
		m_EditorDoc.designMode="on";
    }
    m_EditorDoc.open();
    m_EditorDoc.write(PageTemp);
    m_EditorDoc.close();
    
    IMCore.Utility.AttachEvent(
        m_EditorDoc,"mousedown",
        function()
        {
            try
            {
		        if(m_EditorDoc.activeElement == null)
		        {
		            m_EditorWindow.focus();
		        }
		        else
		        {
		           m_EditorDoc.activeElement.focus();
		        }
		    }
		    catch(ex)
		    {
		    }
        }
    );
	
	m_EditorDoc.onkeydown = function(e)
	{
		var evt = new IMCore.Event(e,m_EditorWindow);
		if(evt.GetEvent().keyCode==116 || (evt.GetEvent().ctrlKey && evt.GetEvent().keyCode==82))
		{
			evt.GetEvent().keyCode=0; 
			evt.GetEvent().returnValue=false;
			return false;
		}
		if(evt.GetEvent().keyCode == 70 && evt.GetEvent().ctrlKey && !evt.GetEvent().altKey && !evt.GetEvent().shiftKey)
		{
			evt.GetEvent().keyCode=0; 
			evt.GetEvent().returnValue=false;
			return false;
		}
	}
	
	var range=null;
	
	This.SaveSelection=function()
	{
		if(IMCore.GetBrowser()=="IE")
		{
			range=m_EditorDoc.selection.createRange();
			if(range.parentElement().document!=m_EditorDoc)
			{
				range = null;
			}
		}
		else if(IMCore.GetBrowser()=="Firefox" || IMCore.GetBrowser()=="Chrome")
		{
			var sel=m_EditorWindow.getSelection();
			if(sel.rangeCount>0) range=sel.getRangeAt(0);else range = null;
		}
	}
	
	This.GetSelectionHtml=function()
	{
		if(IMCore.GetBrowser()=="IE")
		{
			var r=m_EditorDoc.selection.createRange();
			if(r.htmlText!=undefined) return r.htmlText;else return "";
		}
		else if(IMCore.GetBrowser()=="Firefox" || IMCore.GetBrowser()=="Chrome")
		{
			var sel = m_EditorWindow.getSelection();
			if (sel.rangeCount > 0)
			{
				var r = null;
				r = sel.getRangeAt(0);
				return IMCore.Utility.GetInnerHTML(r.cloneContents().childNodes);
			}
			else
			{
				return "";
			}
		}
		else
		{
			return "";
		}
	}
	
	This.ReplaceSaveSelection=function(html)
	{
		if(range!=null)
		{
			if(IMCore.GetBrowser()=="IE")
			{
				if(range.pasteHTML!=undefined)
				{
					range.select();
					range.pasteHTML(html);
					return true;
				}
			}
			else if(IMCore.GetBrowser()=="Firefox" || IMCore.GetBrowser()=="Chrome")
			{
				if(range.deleteContents != undefined && range.insertNode!=undefined)
				{
					var temp=m_EditorDoc.createElement("DIV");
					temp.innerHTML=html;
					
					var elems=[];
					for(var i = 0 ;i<temp.childNodes.length;i++)
					{
						elems.push(temp.childNodes[i]);
					}
					
					range.deleteContents();
					
					for(var i in elems)
					{
						temp.removeChild(elems[i]);
						range.insertNode(elems[i]);
					}
					return true;
				}
			}
		}
		return false;
	}
    
    this.Blur=function()
    {
		m_Editor.Blur();
    }
		
	this.Append=function(content)
	{
	    m_EditorDoc.body.innerHTML+=content;
	}
	
	this.GetValue=function()
	{
		return m_EditorDoc.body.innerHTML;
	}
	
	this.SetValue=function(newValue)
	{
		if(newValue!=undefined && newValue!=null) 
		{
			m_EditorDoc.body.innerHTML=newValue;
		}
	}
	
	this.Focus=function()
	{
		m_EditorWindow.focus();
	}
	
	this.GetDocument=function()
	{
		return m_EditorDoc;
	}
	
	this.GetFrame=function()
	{
		return m_Frame;
	}
	
	this.GetWindow=function()
	{
		return m_EditorWindow;
	}
	
	this.OnKeyDown=new IMCore.Delegate();
	
	IMCore.Utility.AttachEvent(
		m_EditorDoc,
		"keydown",
		function(evt)
		{
			if(evt==undefined) evt=m_EditorWindow.event;
			This.OnKeyDown.Call(evt);
		}
	);
	
    if(config.StyleSheet)
		m_Editor.Link("StyleSheet",config.StyleSheet,"text/css");
    
    This.SetCss("richEditor");
    
    function Editor()
    {
		var editor=this;
	    
		var editorConfig={
			Left:0,Top:24,Width:This.GetClientWidth(),Height:This.GetClientHeight()-24,
			BorderWidth:0,Css:'editor',
			Parent:This,
			AnchorStyle:Controls.AnchorStyle.All
		};
		
		Controls.Frame.call(this,editorConfig);
	    
		var Base={
			GetType:this.GetType,
			is:this.is
		}
    
		editor.is=function(type){return type==this.GetType()?true:Base.is(type);}
		editor.GetType=function(){return "RichEditor.Editor";}
    }
    
}


Module.RichEditor=RichEditor;
		