import React from 'react';
import { render, screen } from '@testing-library/react';
import App from './App';

test('renders AnchorzUp application', () => {
  render(<App />);
  const titleElement = screen.getByText(/AnchorzUp/i);
  expect(titleElement).toBeInTheDocument();
});
