let instance;

// Init state
let initState = {
    isOpenMenu: false
};

class Store {
    constructor() {
        if (instance) {
            throw new Error("You have created on instance");
            return;
        }

        instance = this;
    }

    getState() {
        return { ...initState };
    }

    // Param obj like {stateName: value}
    setState(obj) {
        initState = {
            ...initState,
            ...obj
        };
        return;
    }
}

const store = new Store();

$(".mobile-nav__menu-btn").on("click", () => {
    const navBgDark = $(".nav-bg-dark");
    const mobileNavList = $(".mobile-nav__list");

    // User open menu
    if (!store.getState().isOpenMenu) {
        // Show dark bg
        navBgDark.css("display", "block");

        // Show menu
        mobileNavList.css("right", "0");

        store.setState({ isOpenMenu: true });

        return;
    }

    // User already opened menu
    // Hide dark bg
    navBgDark.css("display", "none");

    // Hide menu
    mobileNavList.css("right", "-100%");

    store.setState({ isOpenMenu: false });
});