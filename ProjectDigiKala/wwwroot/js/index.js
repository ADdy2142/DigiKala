$(document).ready(function () {
    $(".search-label").click(function () {
        if ($(".search-input").val() !== "") {
            $(".search-form").submit();
        }
    });

    // Open/Close specification list
    $(".product-list-side-specs-toggle").click(function () {
        let parent = $(this).parent().parent();
        let child = parent.children(".product-list-side-specs-values");
        if (child.hasClass("show-specs")) {
            child.removeClass("show-specs");
        } else {
            child.addClass("show-specs");
        }
    });

    // Set value of selected specification
    $(".product-list-side-specs-toggle.mdi-plus-box").click(function () {
        let element = $(this).next(".product-list-side-specs-toggle.mdi-minus-box");
        $(element).css("display", "inline-block");
        $(this).css("display", "none");
    });
    $(".product-list-side-specs-toggle.mdi-minus-box").click(function () {
        let element = $(this).prev(".product-list-side-specs-toggle.mdi-plus-box");
        $(element).css("display", "inline-block");
        $(this).css("display", "none");
    });

    // Load tab content
    $(".product-tab-headers-item").click(function () {
        let targetTabContentId = $(this).data("tab");
        let targetTabContent = $("#" + targetTabContentId);
        let curTabContent = $(".product-tab-content.active-tab");

        $(".product-tab-headers-item.active-tab").removeClass("active-tab");
        $(curTabContent).removeClass("active-tab");
        $(this).addClass("active-tab");
        targetTabContent.addClass("active-tab");
    });

    // Highlight the selected address
    $(".order-form-group .order-form-label").click(function () {
        let curActiveAddress = $(".order-form-group.selected-order-form-group");

        $(".order-form-group .order-form-input:checked").removeAttr("checked");

        $(this).parent().children(".order-form-input").attr("checked", true);

        curActiveAddress.removeClass("selected-order-form-group");
        $(this).parent().addClass("selected-order-form-group");
    });

    // Highlight the selected shipment type
    $(".shipment-type .order-form-content-type-item").click(function () {
        //let parent = $(this).parent();
        let curActiveShipment = $(".shipment-type .order-form-content-type-item.active-shipment");

        $(".shipment-type .order-form-content-type-item-input:checked").removeAttr("checked");

        $(this).children(".shipment-type .order-form-content-type-item-input").attr("checked", true);

        curActiveShipment.removeClass("active-shipment");
        $(this).addClass("active-shipment");
    });

    // Highlight the selected payment type
    $(".payment-type .order-form-content-type-item").click(function () {
        //let parent = $(this).parent();
        let curActiveShipment = $(".payment-type .order-form-content-type-item.active-shipment");

        $(".payment-type .order-form-content-type-item-input:checked").removeAttr("checked");

        $(this).children(".payment-type .order-form-content-type-item-input").attr("checked", true);

        curActiveShipment.removeClass("active-shipment");
        $(this).addClass("active-shipment");
    });

    // Show/Hide register login container
    //$(".register-login-btn, .register-login-content-close").click(function (e) {
    //    e.preventDefault();
    //    let container = $(".register-login-content");
    //    container.toggleClass("hide-register-login-content");
    //});

    // Set the value of checkbox inputs
    $("input[type='checkbox']").on("change", function () {
        $(this).val($(this).is(":checked") ? "true" : "false");
    });

    // setInterval(sliderPager, 5000);

    effects();
});

function effects() {
    // Slider
    let slider = $(".home-gallery-wrap");
    slider.mouseout(function () {
        $(".home-gallery-btn.right").addClass("hide-slider-btn");
        $(".home-gallery-btn.left").addClass("hide-slider-btn");
    });
    slider.mouseover(function () {
        $(".home-gallery-btn.right").removeClass("hide-slider-btn");
        $(".home-gallery-btn.left").removeClass("hide-slider-btn");
    });

    // let sliders = $(".home-gallery-item");
    // let curSlider = $(".home-gallery-item.active-slide");
    // let curSliderIndex = 0;
    // setInterval(function () {
    //     if (curSliderIndex < sliders.length) {
    //         curSlider.removeClass("active-slide");
    //         curSlider = curSlider.next();
    //         curSlider.addClass("active-slide");
    //         curSliderIndex++;
    //     } else {
    //         curSlider.removeClass("active-slide");
    //         curSlider = sliders[0];
    //         curSlider.addClass("active-slide");
    //     }
    // }, 5000);
}

function sliderPager() {
    let sliderPages = $(".home-gallery-pager-btn");
    let curPage = $(".home-gallery-pager-btn.active-page");
    let curPageIndex = parseInt(curPage.data("page"));
    let nextPage;

    if (curPageIndex < sliderPages.length) {
        nextPage = curPage.next();
    } else {
        nextPage = sliderPages[0];
    }

    curPage.removeClass("active-page");
    nextPage.addClass("active-page");
}