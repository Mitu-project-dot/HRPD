if (!self.loaded_core) { // prevents double loading
    /* Combined Core.js version 1.1 */

    /* Start of IBS Core */
    var loaded_core = true;
    var js_revision_date = "2010-05-19"; // official version date - do not modify
    if (!self.rotation_delay) var rotation_delay = 6; // in seconds, *1000ms
    var timers = new Array();

    /* QUERY STRING FETCHING, e.g. http://myurl.com/path/?myvar=yes, querystr("myvar") returns "yes" */
    function querystr(varname) {
        var query = window.location.search.substring(1);
        if (!varname) return query;
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (pair[0] == varname)
                return pair[1];
        }
    }

    function ctrl_playpause(selector, sub_selector, obj, play) {
        obj.className = play ? "pause" : "play";
        obj.onmousedown = function () { return ctrl_playpause(selector, sub_selector, this, !play) };
        if (play)
            timers[selector] = window.setTimeout("ctrl_advance('" + selector + "','" + sub_selector + "',true,true)", rotation_delay * 1000);
        else
            timers[selector] = window.clearTimeout(timers[selector]);
        return false;
    }

    function ctrl_advance(selector, sub_selector, forward, auto) {
        var $new = forward ? $(selector + ".selected").next(sub_selector) : $(selector + ".selected").prev(sub_selector);
        $new = $new.length == 0 ? forward ? $(selector + ":first") : $(selector + ":last") : $new;
        $(selector).removeClass("selected");
        $new.addClass("selected");
        if (auto)
            timers[selector] = window.setTimeout("ctrl_advance('" + selector + "','" + sub_selector + "',true,true)", rotation_delay * 1000);
        else if ($(selector.substr(0, selector.indexOf(" ")) + " .controls a.pause").length > 0)
            ctrl_playpause(selector, sub_selector, $(selector.substr(0, selector.indexOf(" ")) + " .controls a.pause").get(0), false)
        return false;
    }

    function controls_init(selector, sub_selector) {
        var root = selector.substr(0, selector.indexOf(" "));
        $(selector + ":first").addClass("selected");
        $(root + " .controls a.next").mousedown(function () { return ctrl_advance(selector, sub_selector, true); });
        if ($(root + " .controls a.pause").length > 0) $(root + " .controls a.pause").get(0).onmousedown = function () { return ctrl_playpause(selector, sub_selector, this, false); };
        if ($(root + " .controls a.play").length > 0) $(root + " .controls a.play").get(0).onmousedown = function () { return ctrl_playpause(selector, sub_selector, this, true); };
        $(root + " .controls a.prev").mousedown(function () { return ctrl_advance(selector, sub_selector, false); });
    }

    function overlay_write(content) { $("#overlay-dync").html(content); }
    function overlay_writefrom(url) { $("#overlay-dync").load(url); }

    $(document).ready(function () {
        var str_controls = "<div class=\"controls\"><a class=\"prev\" href=\"javascript:void(0)\" title=\"previous\"></a><a class=\"play\" href=\"javascript:void(0)\" title=\"pause\"></a><a class=\"next\" href=\"javascript:void(0)\" title=\"next\"></a></div>";
        if (querystr("pageclass")) $("#page").addClass(querystr("pageclass").replace(/\+/gi, " ")); //page classes injection

        /* LAYOUT INTERACTION CHAINING */
        $(".layout-popup").parent().addClass("popup");
        $(".layout-124.band-right").addClass("br-f");
        $(".band-right.search").addClass("br-s");
        $(".br-f.br-s").addClass("br-fs");
        $(".welcome.band-right").addClass("br-w");
        $(".br-w.layout-394").addClass("br-w394");
        $(".layout-412.welcome.band-right.search").text("really??").addClass("ws-200").addClass("align-center");
        $("div p:last-child,div ul:last-child").addClass("no-margins"); /* testing */
        $(".zebra-even tr:even").addClass("shaded");
        $(".zebra-odd tr:odd").addClass("shaded");
        $("#search .arrow-button").click(function () { $(this).parent("form").submit(); });

        /* MENU IE6 TOP NAV */
        $("#topnav > li").mouseover(function () {
            $(this).children("ul").show();
            $(this).css("background", "#75caeb");
        });
        $("#topnav > li").mouseout(function () {
            $(this).children("ul").hide();
            $(this).css("background", "transparent");
        });

        /* MBAND CONTROLS */
        $("#band .frame").parent().append(str_controls);
        controls_init("#band .frame", ".frame");

        /* METRICS CONTROLS */
        $(".metrics .shaded ul").before(str_controls);
        controls_init(".metrics .shaded ul li", "");
        $(".metrics .shaded ul li a").click(function (event) { event.preventDefault(); });
        $(".metrics .shaded ul li a").mousedown(function () {
            $(this).parent().siblings().removeClass("selected");
            $(this).parent().addClass("selected");
            ctrl_playpause(".metrics .shaded ul li", "", $(".metrics .controls a.pause").get(0), false)
        });

        /* INIT ROTATIONS */
        $(".controls a.play").each(function () { this.onmousedown(); });

        /* TAB CONTROLS */
        $(".tabbed ul.tabs li:first-child").addClass("selected");
        $(".tabbed div.panel:nth-child(2)").addClass("selected");
        $(".tabbed ul.tabs a").click(function (event) { event.preventDefault(); });
        $(".tabbed ul.tabs a").mousedown(function () {
            $(this).parent().siblings().removeClass("selected");
            $(this).parent().addClass("selected");
            $($(this).attr("href")).siblings().removeClass("selected");
            $($(this).attr("href")).addClass("selected");
            $(".tabbed div.panel").removeClass("selected");
            $(".tabbed div.panel:nth-child(" + ($(this).parent().index() + 2) + ")").addClass("selected");
            return false;
        });

        /* OVERLAY CONTROL <a class="show-overlay" href="#id"> */
        $(".show-overlay").click(function () {
            $("#overlay .frame").removeClass("selected");
            $($(this).attr("href")).addClass("selected");
            $("#overlay p:last-child").addClass("no-margins");
            if ($("#overlay .selected").length == 0) $("#overlay-dync").addClass("selected");
            $("#overlay").show();
            $("#overlay .inside").css("top", ($(window).height() - $("#overlay .inside").height()) / 3 + "px");
        });
        $("#overlay .close").click(function () { $("#overlay").hide() });

        /* ICON INJECTION <a class="icon-pdf" href="my.pdf"> */
        $("a[class*=icon-]").each(function () {
            var icon_class = $(this).get(0).className.replace(/\s*icon-(\w+)/gi, "$1");
            $(this).addClass("icon").removeClass("icon-" + icon_class);
            $(this).prepend("<em class='" + icon_class + "'></em>");
        });

        /* SEARCH FIELD TOGGLING */
        var original_search = $("#search input[type='text']").val();
        $("#search input[type='text']").focus(function () { if ($(this).val() == original_search) $(this).val("") });
        $("#search input[type='text']").blur(function () { if ($(this).val() == "") $(this).val(original_search) });

        /* CORE COMPILED FUNCTIONS */
    });
}