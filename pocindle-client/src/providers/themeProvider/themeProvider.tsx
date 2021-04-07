import React, { useState } from 'react';

interface IThemeNames {
  light: string;
  dark: string;
}

type ThemeContext = {
  theme: any;
  switchTheme: (themeNameValue: keyof IThemeNames) => void;
};

const initialContext: ThemeContext = {
  theme: {},
  switchTheme: () => {
    return;
  },
};

const themes = {
  light: {
    test: 'testlight',
  },
  dark: {
    test: 'testdark',
  },
};

export const ThemeContext = React.createContext(initialContext);

export const ThemeProvider: React.FC = ({ children }) => {
  const [theme, setTheme] = useState<unknown>(themes.light);
  const [themeName, setThemeName] = useState<string>('light');

  const switchTheme = (themeNameValue: keyof IThemeNames) => {
    console.log(themeNameValue);
    setTheme(themes[themeNameValue]);
    setThemeName(themeName);
  };

  return (
    <ThemeContext.Provider value={{ theme, switchTheme }}>
      {children}
    </ThemeContext.Provider>
  );
};
