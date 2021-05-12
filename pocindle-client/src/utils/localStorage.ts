const languageKey = 'lang';
const jwtKey = 'jwt';

export const setLanguageToLocalStorage = (value: string): void => {
  localStorage.setItem(languageKey, value);
};

export const getLanguageFromLocalStorage = (): string => {
  const value = localStorage.getItem(languageKey) || '';
  return value;
};

export const setJwtToLocalStorage = (value: string): void => {
  localStorage.setItem(jwtKey, value);
};

export const getJwtFromLocalStorage = (): string => {
  const value = localStorage.getItem(jwtKey) || '';
  return value;
};
