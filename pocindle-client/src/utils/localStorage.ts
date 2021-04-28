const languageKey = 'lang';

export const setLanguageToLocalStorage = (value: string): void => {
  localStorage.setItem(languageKey, value);
};

export const getLanguageFromLocalStorage = (): string => {
  const value = localStorage.getItem(languageKey) || '';
  return value;
};
