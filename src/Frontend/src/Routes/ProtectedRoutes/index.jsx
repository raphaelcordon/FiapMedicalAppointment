import { useSelector } from "react-redux"
import { Navigate, Outlet, useLocation } from "react-router-dom"

const ProtectedRoutes = () => {

    const location = useLocation()
    const isUserLoggedIn = useSelector((state) => state.user.token)

    return !isUserLoggedIn ? (
        // Navigate to the /login route and replace the current history entry
        // replace true will replace the current history entry instead of adding a new one
        // this prevents users from an infinite loop of redirects when clicking the back button
        <Navigate to="/" replace state={{ from: location.pathname }} />
    ) : (
        <div className="flex-grow">
            <Outlet />
        </div>
    )
}

export default ProtectedRoutes
