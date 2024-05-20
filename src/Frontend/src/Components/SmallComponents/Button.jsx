const Button = ({ children, className, ...props }) => {
  // Default background color class
  const defaultBgClass = "bg-secondary";

  // Combine default classes with custom classes
  const combinedClasses = `${defaultBgClass} ${className}`;

  return (
    <button className={`btn btn-md-wide px-20 text-base-100 ${combinedClasses}`} {...props}>
      {children}
    </button>
  );
};

export default Button;

/* const Button = ({ children }) => {
  return (
    <>
      <button className="btn bg-secondary px-16 my-4 md:my-8 ">{children}</button>
    </>
  );
};

export default Button;
 */
