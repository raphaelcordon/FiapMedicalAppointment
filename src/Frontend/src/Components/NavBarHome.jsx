import { useEffect, useState } from "react";
import NavBarLink from "./SmallComponents/NavBarLinks";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faHome, faRightFromBracket, faRocket, faUser } from "@fortawesome/free-solid-svg-icons";
import { useDispatch, useSelector } from "react-redux";
import useGetMeUser from "../Hooks/useGetMeUser.js";
import { NavLink, useNavigate } from "react-router-dom";
import { logoutUser } from "../Store/slices/userSlice.js";
import useGetMedicalSpecialties from "../Hooks/useGetMedicalSpecialties.js";

const NavBarHome = () => {
  const dispatch = useDispatch();
  const user = useSelector((state) => state.user.userData);
  const token = useSelector((state) => state.user.token);
  const { getUser } = useGetMeUser();
  const { getMedicalSpecialties } = useGetMedicalSpecialties();
  const navigate = useNavigate();

  useEffect(() => {
    if ((!user || (Array.isArray(user) && user.length === 0)) && window.localStorage.getItem("token")) {
      getUser();
    }
    getMedicalSpecialties();
  }, [user, token, getUser, getMedicalSpecialties]);

  const logoutHandler = () => {
    dispatch(logoutUser());
    window.localStorage.removeItem("token");
    navigate("/");
  };

  return (
    <div className="navbar border border-base-300 bg-base-100/50 shadow-lg backdrop-blur-2xl fixed bottom-0 left-0 w-full z-10 md:top-0 md:bottom-auto flex justify-between">
      <div className="flex items-center justify-around w-1/2 pr-5">
        <NavBarLink to="/" className="flex items-center">
          <FontAwesomeIcon icon={faHome} />
          <span>Home</span>
        </NavBarLink>

        {user ? (
          <>
            {user.roles.includes("Doctor") && (
              <>
                <NavBarLink to="/doctor" className="flex items-center">
                  <FontAwesomeIcon icon={faRocket} />
                  <span>Doctor</span>
                </NavBarLink>
                <NavBarLink to="/medicalspecialty" className="flex items-center">
                  <FontAwesomeIcon icon={faRocket} />
                  <span>Specialties</span>
                </NavBarLink>
                <NavBarLink to="/appointmentdoctor" className="flex items-center">
                  <FontAwesomeIcon icon={faRocket} />
                  <span>Appointments</span>
                </NavBarLink>
              </>
            )}
            {user.roles.includes("Patient") && (
            <>
              <NavBarLink to="/patient" className="flex items-center">
                <FontAwesomeIcon icon={faRocket} />
                <span>Patient</span>
              </NavBarLink>
              <NavBarLink to="/appointmentpatient" className="flex items-center">
                <FontAwesomeIcon icon={faRocket} />
                <span>Appointments</span>
              </NavBarLink>
            </>
            )}
          </>
        ) : (
          <>
            <NavBarLink to="/registration" className="flex items-center">
              <FontAwesomeIcon icon={faRocket} />
              <span>Register</span>
            </NavBarLink>
            <NavBarLink to="/login" className="flex items-center">
              <FontAwesomeIcon icon={faUser} />
              <span>Login</span>
            </NavBarLink>
          </>
        )}
      </div>
      {user && (
        <NavLink
          to=""
          onClick={(e) => {
            e.preventDefault();
            logoutHandler();
            navigate("/");
          }}
        >
          <div className="flex flex-col items-center pl-15 hover:font-bold mx-2 my-2 md:mx-10 lg:mx-8 lg:my-0 xl:mx-12">
            <FontAwesomeIcon icon={faRightFromBracket} />
            <span>Logout</span>
          </div>
        </NavLink>
      )}
    </div>
  );
};

export default NavBarHome;
