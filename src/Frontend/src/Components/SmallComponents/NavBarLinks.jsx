import { Link, useLocation } from "react-router-dom";

const NavBarLink = ({ to, children }) => {
  const location = useLocation();

  return (
    <Link
      to={to}
      className={`flex flex-col items-center pl-15 hover:font-bold  mx-2 my-2 md:mx-10 lg:mx-8 lg:my-0 xl:mx-12 ${location.pathname === to ? "text-secondary" : ""}`}
    >
      {children}
    </Link>
  );
};

export default NavBarLink;
