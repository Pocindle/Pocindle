import React, { useState } from 'react';

interface ILanguageNames {
  ru: string;
  eng: string;
}

type LanguageContext = {
  language: any;
  switchLanguage: (languageNameValue: keyof ILanguageNames) => void;
};

const initialContext: LanguageContext = {
  language: {},
  switchLanguage: () => {
    return;
  },
};

const languages = {
  ru: {
    name: 'русский',
    navbar: {
      articles: 'Статьи',
      logOut: 'Выйти',
    },
    footer: {
      about: 'О сайте',
      siteTheme: 'Тема сайта:',
      language: 'Язык:',
    },
  },
  eng: {
    name: 'english',
    navbar: {
      articles: 'Articles',
      logOut: 'Log out',
    },
    footer: {
      about: 'About',
      siteTheme: 'Site Theme:',
      language: 'Language:',
    },
  },
};

export const LanguageContext = React.createContext(initialContext);

export const LanguageProvider: React.FC = ({ children }) => {
  const [language, setLanguage] = useState<unknown>(languages.ru);

  const switchLanguage = (languageNameValue: keyof ILanguageNames) => {
    console.log(languageNameValue);
    setLanguage(languages[languageNameValue]);
  };

  return (
    <LanguageContext.Provider value={{ language, switchLanguage }}>
      {children}
    </LanguageContext.Provider>
  );
};
