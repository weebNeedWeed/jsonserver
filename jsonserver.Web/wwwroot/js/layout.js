let isOpenMenu = false;
$(".mobile-nav__menu-btn").on("click", () => {
    const navBgDark = $(".nav-bg-dark");
    const mobileNavList = $(".mobile-nav__list");

    // User open menu
    if (!isOpenMenu) {
        // Show dark bg
        navBgDark.css("display", "block");

        // Show menu
        mobileNavList.css("right", "0");

        isOpenMenu = true;

        return;
    }

    // User already opened menu
    // Hide dark bg
    navBgDark.css("display", "none");

    // Hide menu
    mobileNavList.css("right", "-100%");

    isOpenMenu = false;
});