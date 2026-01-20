/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: '#007aff',
        secondary: '#f8f9fa',
        accent: '#ff5a5f',
      }
    },
  },
  plugins: [],
}