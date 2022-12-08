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
  plugins: [require("daisyui")],
}