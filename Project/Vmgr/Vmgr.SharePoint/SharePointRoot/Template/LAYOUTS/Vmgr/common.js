
$(document).ready(function () {
    OnWindowResize();
});

addEvent(window, "resize", OnWindowResize);

function OnWindowResize() {
    var height = getViewportHeight();
    var bodyContentBody = document.getElementById("body-content-body");
    var mainContent = document.getElementById("mainContent");
    var menuContent = document.getElementById("menuContent");

    if (bodyContentBody != null) {
        var top = bodyContentBody.offsetTop;
        var bodyHeight = height - top - 4;
        bodyContentBody.style.height = bodyHeight + 'px';

    }

    if (mainContent != null) {
        var top = mainContent.offsetTop;
        var mainContentHeight = height - top - 2;
        mainContent.style.height = mainContentHeight + 'px';
        mainContent.style.maxHeight = mainContentHeight + 'px';
    }

    if (menuContent != null) {
        var top = menuContent.offsetTop;
        var menuContentHeight = height - top - 100;
        menuContent.style.height = menuContentHeight + 'px';
        menuContent.style.maxHeight = menuContentHeight + 'px';
    }
}

/**
* COMMON DHTML FUNCTIONS
* These are handy functions I use all the time.
*
* By Seth Banks (webmaster at subimage dot com)
* http://www.subimage.com/
*
* Up to date code can be found at http://www.subimage.com/dhtml/
*
* This code is free for you to use anywhere, just keep this comment block.
*/

/**
* X-browser event handler attachment and detachment
* TH: Switched first true to false per http://www.onlinetools.org/articles/unobtrusivejavascript/chapter4.html
*
* @argument obj - the object to attach event to
* @argument evType - name of the event - DONT ADD "on", pass only "mouseover", etc
* @argument fn - function to call
*/
function addEvent(obj, evType, fn) {
    if (obj.addEventListener) {
        obj.addEventListener(evType, fn, false);
        return true;
    } else if (obj.attachEvent) {
        var r = obj.attachEvent("on" + evType, fn);
        return r;
    } else {
        return false;
    }
}
function removeEvent(obj, evType, fn, useCapture) {
    if (obj.removeEventListener) {
        obj.removeEventListener(evType, fn, useCapture);
        return true;
    } else if (obj.detachEvent) {
        var r = obj.detachEvent("on" + evType, fn);
        return r;
    } else {
        alert("Handler could not be removed");
    }
}

function getViewportHeight() {
    if (window.innerHeight != window.undefined) return window.innerHeight;
    if (document.compatMode == 'CSS1Compat') return document.documentElement.clientHeight;
    if (document.body) return document.body.clientHeight;

    return window.undefined;
}

function getViewportWidth() {
    var offset = 17;
    var width = null;
    if (window.innerWidth != window.undefined) return window.innerWidth;
    if (document.compatMode == 'CSS1Compat') return document.documentElement.clientWidth;
    if (document.body) return document.body.clientWidth;
}

/**
* Gets the real scroll top
*/
function getScrollTop() {
    if (self.pageYOffset) // all except Explorer
    {
        return self.pageYOffset;
    }
    else if (document.documentElement && document.documentElement.scrollTop)
    // Explorer 6 Strict
    {
        return document.documentElement.scrollTop;
    }
    else if (document.body) // all other Explorers
    {
        return document.body.scrollTop;
    }
}

function getScrollLeft() {
    if (self.pageXOffset) // all except Explorer
    {
        return self.pageXOffset;
    }
    else if (document.documentElement && document.documentElement.scrollLeft)
    // Explorer 6 Strict
    {
        return document.documentElement.scrollLeft;
    }
    else if (document.body) // all other Explorers
    {
        return document.body.scrollLeft;
    }
}


function OnCenterLoadingPanel(loadingPanel) {
    var pageHeight = document.documentElement.scrollHeight;
    var viewportHeight = document.documentElement.clientHeight;

    if (pageHeight > viewportHeight) {
    }

    loadingPanel.style.height = pageHeight + "px";
    var pageWidth = document.documentElement.scrollWidth;
    var viewportWidth = document.documentElement.clientWidth;

    if (pageWidth > viewportWidth) {
        loadingPanel.style.width = pageWidth + "px";
    }

    // the following Javascript code takes care of centering the RadAjaxLoadingPanel
    // background image, taking into consideration the scroll offset of the page content

    if ($telerik.isSafari) {
        var scrollTopOffset = document.body.scrollTop;
        var scrollLeftOffset = document.body.scrollLeft;
    }
    else {
        var scrollTopOffset = document.documentElement.scrollTop;
        var scrollLeftOffset = document.documentElement.scrollLeft;
    }

    var loadingImageWidth = 55;
    var loadingImageHeight = 55;

    loadingPanel.style.backgroundPosition = (parseInt(scrollLeftOffset) + parseInt(viewportWidth / 2) - parseInt(loadingImageWidth / 2)) + "px " + (parseInt(scrollTopOffset) + parseInt(viewportHeight / 2) - parseInt(loadingImageHeight / 2)) + "px";
}

function FunctionArray() {
    this.functionArray = new Array();
}

FunctionArray.prototype.add = function (k, f) {
    if (typeof f != "function") {
        f = new Function(f);
    }
    this.functionArray[this.functionArray.length] = { key: k, func: f };
}

FunctionArray.prototype.getLength = function () {
    return this.functionArray.length;
}

FunctionArray.prototype.execute = function (k, d) {
    for (var i = 0; i < this.functionArray.length; i++) {
        if (this.functionArray[i].key == k)
            this.functionArray[i].func(d);
    }
}

FunctionArray.prototype.executeAll = function (d) {
    for (var i = 0; i < this.functionArray.length; i++) {
        this.functionArray[i].func(d);
    }
}

