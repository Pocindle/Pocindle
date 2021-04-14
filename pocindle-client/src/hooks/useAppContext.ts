import { useContext } from 'react';
import { ThemeContext, LanguageContext } from '../providers';

const useAppContext = () => {
  const { theme, switchTheme } = useContext(ThemeContext);
  const { language, switchLanguage } = useContext(LanguageContext);

  return { theme, switchTheme, language, switchLanguage };
};

export default useAppContext;
