if (!self.loaded_core) { // prevents double loading
    var loaded_core = true;
    var js_revision_date = "2010-06-28"; // official version date - do not modify
    if (!self.rotation_delay) var rotation_delay = 6; // in seconds, *1000ms
    var timers = new Array();
    var uniqueid = 0;

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

    function ctrl_playpause(region, play) {
        $(region).find(".controls a.play, .controls a.pause").each(function () {
            this.className = play ? "pause" : "play";
            this.onmousedown = function () { return ctrl_playpause(region, !play) };
        });
        timers[region] = play ? window.setTimeout("ctrl_advance('" + region + "',true,true)", rotation_delay * 1000) : window.clearTimeout(timers[region]);
        return false;
    }

    function ctrl_advance(region, forward, auto) {
        var target = $(region).find(".controls a").data("loc").target;
        var $new = forward ? $(target + ".selected").next(target) : $(target + ".selected").prev(target);
        $new = $new.length == 0 ? forward ? $(target + ":first") : $(target + ":last") : $new;
        $(target).removeClass("selected");
        $new.addClass("selected");
        if (auto) timers[region] = window.setTimeout("ctrl_advance('" + region + "',true,true)", rotation_delay * 1000);
        return false;
    }

    function controls_init(region, target) {
        $(region + " .controls a").each(function () { // and runs once per control set
            var region_id = $(this).parents(region).attr("rel") || "controls_region" + uniqueid++;
            $(this).parents(region).attr("rel", region_id);
            region_id = "[rel=" + region_id + "]";
            $(region_id + " " + target + ":first").addClass("selected");
            $(this).parents(".controls").find("a").data("loc", { region: region_id, target: "." + region_id + " " + target });
            this.onmousedown = this.className == "play" ? function () { return ctrl_playpause($(this).data("loc").region, true); } : function () { return ctrl_advance($(this).data("loc").region, $(this).hasClass("next")); };
        });
    }
    function overlay_write(content) { $("#overlay-dync").html(content); }
    function overlay_writefrom(url) { $("#overlay-dync").load(url); }

    function checkforSearchKeywords(searchValue) {
        if (searchValue == "" || searchValue == "Find with OneSearch") {
            alert("Please enter at least one keyword for your search");
            return false;
        }
        return true;
    }

    $(document).ready(function () {
        /* toplink and search overrides */
        $("#toplinks li:eq(0) a").html("Inside Home").attr("href", "http://inside.chevron.com");
        $("#toplinks li:eq(1) a").html("OneSearch").attr("href", "http://onesearch.chevron.com");
        $("#search").attr("action", "http://onesearch.chevron.com/Pages/results.aspx");
        $("#searchfield").attr({ name: 'k', value: 'Find with OneSearch' });

        var str_controls = "<div class=\"controls\"><a class=\"prev\" href=\"javascript:void(0)\" title=\"previous\"></a><a class=\"play\" href=\"javascript:void(0)\" title=\"play/pause\"></a><a class=\"next\" href=\"javascript:void(0)\" title=\"next\"></a></div>";
        if (querystr("pageclass")) $("#page").get(0).className = querystr("pageclass").replace(/\+/gi, " "); //page classes injection
        //if (querystr("imageSrc")) $(".layout-popup #sitename").attr("src",querystr("imageSrc")+".png").attr("width",querystr("imageWidth"));

        /* LAYOUT INTERACTION CHAINING */
        $(".effect-gradient").parent().addClass("effect-gradient");
        $(".layout-popup").parent().addClass("layout-popup-wrapper");
        $(".frame, #band div, fieldset").children(":last-child:not(ul,ol)").addClass("no-margins");
        $(".panel, .inset .shaded").children(":last-child").addClass("no-margins");
        $(".zebra-even tr:even").addClass("shaded");
        $(".zebra-odd tr:odd").addClass("shaded");
        $(".human-energy #hallmark").attr({ src: 'http://idcscripts.chevron.com/opix/images/hallmark_he.png', width: '88', height: '81', alt: 'Chevron Human Energy' });

        /* MENU IE6 TOP NAV */
        $("#topnav > li").mouseover(function () {
            $(this).children("ul").show();
            $(this).css("background", "#75caeb");
        });
        $("#topnav > li").mouseout(function () {
            $(this).children("ul").hide();
            $(this).css("background", "transparent");
        });

        /* LISTS */
        $("#content ul, #content ol").not("#sidenav,.no-bullet,.linkhandler_omit,.arrows,.bullets,.alpha,.roman").each(function () {
            if ($(this).children("li").length == $(this).children("li:has(a)").length) $(this).addClass("arrows");
        });

        /* MBAND CONTROLS */
        $("#band .frame").parent().append(str_controls);
        controls_init("#band", ".frame");

        /* TAB CONTROLS */
        $(".tabbed ul.tabs li:first-child").addClass("selected");
        $(".tabbed div.panel:nth-child(2)").addClass("selected");
        $(".tabbed ul.tabs a").click(function (event) { event.preventDefault(); });
        $(".tabbed ul.tabs a").mousedown(function () {
            $(this).parent().siblings().removeClass("selected");
            $(this).parent().addClass("selected");
            $($(this).attr("href")).siblings().removeClass("selected");
            $($(this).attr("href")).addClass("selected");
            return false;
        });

        /* METRICS CONTROLS */
        $(".metrics .shaded ul").before(str_controls);
        controls_init(".metrics", ".shaded ul li");
        $(".metrics .shaded ul li a").click(function (event) { event.preventDefault(); });
        $(".metrics .shaded ul li a").mousedown(function () {
            $(this).parent().siblings().removeClass("selected");
            $(this).parent().addClass("selected");
            ctrl_playpause(".metrics .shaded ul li", "", $(".metrics .controls a.pause").get(0), false)
        });

        /* INIT ROTATIONS */
        $(".controls a.play").each(function () { this.onmousedown(); });

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

        /* POPUP CONTROL */
        $(".layout-popup").prepend("<a id='popup-close' class='close' href='#'>Close</a>");
        $(".layout-popup #popup-close").unbind().click(function () { window.close() });

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

    });
}