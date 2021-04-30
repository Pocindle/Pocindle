import React from 'react';
import AppRoute from './routes/appRoute';
import { LanguageProvider } from './providers';

const App: React.FC = () => {
  return (
    <React.Fragment>
      <LanguageProvider>
        <AppRoute />
      </LanguageProvider>
    </React.Fragment>
  );
};

export default App;
