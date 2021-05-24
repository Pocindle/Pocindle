const languageKey = 'lang';
const jwtKey = 'jwt';
const expirationDateKey = 'expirationDate';

export const setLanguageToLocalStorage = (value: string): void => {
  localStorage.setItem(languageKey, value);
};

export const getLanguageFromLocalStorage = (): string => {
  const value = localStorage.getItem(languageKey) || '';
  return value;
};

export const setJwtTokenToLocalStorage = (value: string): void => {
  const expirationDate = new Date();
  localStorage.setItem(jwtKey, value);
  expirationDate.setMinutes(expirationDate.getMinutes() + 59);
  localStorage.setItem(expirationDateKey, JSON.stringify(expirationDate));
};

export const getJwtTokenFromLocalStorage = (): string => {
  const value = localStorage.getItem(jwtKey) || '';
  return value;
};

export const getJwtTokenExpirationDate = (): Date => {
  const value = new Date(
    JSON.parse(localStorage.getItem(expirationDateKey) || '')
  );
  return value;
};

export const removeJwtTokenFromLocalStorage = (): void => {
  localStorage.removeItem(jwtKey);
  localStorage.removeItem(expirationDateKey);
};
