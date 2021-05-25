import React, { useState, useEffect } from 'react';
import './kindleEmailInput.scss';

const KindleEmailInput: React.FC<{
  initialEmail?: string;
  onUpdateEmail: (email: string) => void;
}> = ({ initialEmail, onUpdateEmail }) => {
  const [email, setEmail] = useState(initialEmail || '');

  useEffect(() => {
    setEmail(initialEmail as string);
  }, [initialEmail]);

  const handleMailInputChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setEmail(event.target.value);
  };

  const handleUpdateEmail = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    onUpdateEmail(email);
  };

  return (
    <form onSubmit={handleUpdateEmail} className="kindle-email-input">
      <div className="kindle-email-input__wrapper">
        <div className="kindle-email-input__title-wrapper">
          <span className="kindle-email-input__title">
            {'Current Kindle Email:'}
          </span>
          <span className="kindle-email-input__title-name">
            {initialEmail ? initialEmail : 'Not specified'}
          </span>
        </div>
        <div className="kindle-email-input__form-wrapper">
          <div className="kindle-email-input__element-wrapper">
            <label className="kindle-email-input__label" htmlFor="email">
              Kindle Email:
            </label>
            <input
              name="email"
              id="email"
              placeholder="Your Kindle Email"
              value={email}
              onChange={handleMailInputChange}
              required
              className="kindle-email-input__input"
            />
          </div>
          <button className="kindle-email-input__button">Update mail</button>
        </div>
      </div>
    </form>
  );
};

export default KindleEmailInput;
