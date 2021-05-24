import React, { useState } from 'react';
import {
  setLanguageToLocalStorage,
  getLanguageFromLocalStorage,
} from '../../utils/localStorage';
import languages from '../../utils/languages.json';

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
