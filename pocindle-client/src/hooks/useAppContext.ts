import { useContext } from 'react';
import { LanguageContext } from '../providers';

const useAppContext = () => {
  const { language, switchLanguage } = useContext(LanguageContext);

  return { language, switchLanguage };
};

export default useAppContext;
