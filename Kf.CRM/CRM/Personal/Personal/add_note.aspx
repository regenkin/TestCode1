<meta http-equiv="Content-Type" content="text/html; charset=GB2312" />
<h3 class="popupTitle">新增便签</h3>

<!-- The preview: -->
<div id="previewNote" class="note yellow" style="left: 0; top: 65px; z-index: 1">
    <div class="body"></div>
    <div class="author"></div>
    <div class='delbtn'>x</div>
    <span class="data"></span>
</div>

<div id="noteData">
    <!-- Holds the form -->
    <form action="" method="post" class="note-form">

        <label for="note-body">便签内容</label>
        <textarea name="note-body" id="note-body" class="pr-body" cols="30" rows="10"></textarea>
        <label>Color</label>
        <!-- Clicking one of the divs changes the color of the preview -->
        <div class="color yellow"></div>
        <div class="color blue"></div>
        <div class="color green"></div>
        <!-- The green submit button: -->
        <a id="note-submit" href="" class="green-button">提交</a>
    </form>
</div>
