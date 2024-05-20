import NavBarHome from "../../Components/NavBarHome.jsx";
import { Outlet } from "react-router-dom";
function Layout() {

    return (
        <div className="relative min-h-screen bg-base-300">
            {/* Background element */}
            <div className="absolute inset-0 z-0"></div>

            {/* Content */}
            <div className="flex flex-col min-h-screen relative z-10">
                <NavBarHome/>
                <div className="flex-grow overflow-y-auto pb-[20%] md:pb-3 pt-[10%]">
                    <Outlet />
                </div>
            </div>
        </div>
    );
}

export default Layout;