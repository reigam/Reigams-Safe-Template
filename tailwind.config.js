/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
      './src/**/*.html',
      './src/**/*.fs',
      '.fable-build/**/*.js',
  ],
  theme: {
      extend: {},
  },
  daisyui: {
      darkTheme: false,
      themes: ["light", "dark", "cmyk"],
  },
  plugins: [require("daisyui")],
}