module.exports = {
  env: {
    browser: true,
    node: true,
    es6: true,
  },
  plugins: ['react'],
  extends: [
    'eslint:recommended',
    'plugin:react/recommended',
    'plugin:react-hooks/recommended',
    'plugin:@typescript-eslint/recommended',
  ],
  parser: '@typescript-eslint/parser',
  parserOptions: {
    ecmaVersion: 6,
    sourceType: 'module',
    ecmaFeatures: {
      impliedStrict: true,
      jsx: true,
    },
  },
  rules: {
    'no-unused-vars': 'warn',
    'react/prop-types': 'off',
    quotes: [2, 'single', { avoidEscape: true }],
    semi: 'error',
    'react-hooks/rules-of-hooks': 'error',
    'react-hooks/exhaustive-deps': 'warn',
  },
  settings: {
    react: {
      version: 'latest',
    },
  },
  ignorePatterns: ['dto/'],
};
