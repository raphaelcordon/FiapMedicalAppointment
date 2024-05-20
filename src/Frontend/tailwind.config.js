module.exports = {
  content: ["./src/**/*.{js,ts,jsx,tsx}"],
  daisyui: {
    themes: [
      {
        bumblebee: {
          ...require("daisyui/src/theming/themes")["bumblebee"],
          background: "#FAF7ED",
          "base-300": "#FAF7ED",
        },
      },
    ],
    utils: true,
  },
  plugins: [require("daisyui")],
  theme: {
    extend: {
      // fontFamily: {
      //   sans: ["Varela Round", "sans-serif"],
      // },
      // fontSize: {
      //   base: "0.875rem",
      // },
      // lineHeight: {
      //   normal: "1.4",
      // },
      // Customizing the width, margin, and padding
      width: {
        full: "100%", // Equivalent to width: 100%;
      },
      margin: {
        0: "0", // Equivalent to margin: 0;
      },
      padding: {
        0: "0", // Equivalent to padding: 0;
      },
    },
  },
};

