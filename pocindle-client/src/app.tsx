import React from 'react';
import AppRoute from './routes/appRoute';
import { LanguageProvider } from './providers';

const App: React.FC = () => {
  return (
    <LanguageProvider>
      <AppRoute />
    </LanguageProvider>
  );
};

export default App;
