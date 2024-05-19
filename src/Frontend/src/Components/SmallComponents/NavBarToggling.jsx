const NavBarToggling = ({ children, setActiveTab, tabName, active }) => {
  const className = `cursor-pointer flex flex-col items-center pl-15 hover:font-bold  mx-2 my-2 md:mx-10 lg:mx-8 lg:my-0 xl:mx-12 ${
    active ? "text-secondary" : ""
  }`;

  // This function will be called when the element is clicked.
  const handleOnClick = () => {
    setActiveTab(tabName);
  };

  return (
    <div className={className} onClick={handleOnClick}>
      {children}
    </div>
  );
};

export default NavBarToggling;
