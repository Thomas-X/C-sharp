import Home from "../Pages/Home";
import About from "../Pages/About";
import NotFound from "../Pages/NotFound";

export default {
    home: {
        path: "/",
        component: Home,
        exact: true,
    },
    about: {
        path: "/about",
        component: About,
        exact: true,
    },
    notFound: {
        component: NotFound,
    }
};