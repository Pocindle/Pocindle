import { getJwtTokenExpirationDate } from './localStorage';

export const isDateExpired = (): boolean => {
  const expirationDate = getJwtTokenExpirationDate();
  const currentDate = new Date();
  console.log(currentDate, expirationDate);
  return currentDate > expirationDate;
};

const validateExpirationDate = (onUnsuccessfulValidation: () => void): void => {
  if (isDateExpired()) onUnsuccessfulValidation();
};

export const setExpirationDateInterval = (
  onExpiratedDate: () => void,
  intervalTime: number
): number => {
  return window.setInterval(
    () => validateExpirationDate(onExpiratedDate),
    intervalTime
  );
};
