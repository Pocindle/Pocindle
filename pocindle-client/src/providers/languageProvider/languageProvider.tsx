import React, { useState } from 'react';
import {
  setLanguageToLocalStorage,
  getLanguageFromLocalStorage,
} from '../../utils/localStorage';

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
    name: 'ru',
    navbar: {
      articles: 'Статьи',
      logOut: 'Выйти',
    },
    footer: {
      about: 'О сайте',
      siteTheme: 'Тема сайта:',
      siteLanguage: 'Язык:',
    },
  },
  eng: {
    name: 'eng',
    navbar: {
      articles: 'Articles',
      logOut: 'Log out',
    },
    footer: {
      about: 'About',
      siteTheme: 'Site Theme:',
      siteLanguage: 'Language:',
    },
  },
};

export const LanguageContext = React.createContext(initialContext);

export const LanguageProvider: React.FC = ({ children }) => {
  const [language, setLanguage] = useState<unknown>(
    getLanguageFromLocalStorage() === 'ru' ? languages.ru : languages.eng
  );

  const switchLanguage = (languageNameValue: keyof ILanguageNames) => {
    setLanguage(languages[languageNameValue]);
    setLanguageToLocalStorage(languageNameValue);
  };

  return (
    <LanguageContext.Provider value={{ language, switchLanguage }}>
      {children}
    </LanguageContext.Provider>
  );
};
