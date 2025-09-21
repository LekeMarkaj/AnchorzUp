import React, { useState, useEffect, useRef } from 'react';
import { ChevronDown } from 'lucide-react';
import { shortUrlApi, ShortUrlResponse } from '../services/api';

interface UrlShortenerProps {
  onUrlCreated: (url: ShortUrlResponse) => void;
}

const UrlShortener: React.FC<UrlShortenerProps> = ({ onUrlCreated }) => {
  const [originalUrl, setOriginalUrl] = useState('');
  const [expiresAt, setExpiresAt] = useState('');
  const [selectedExpirationLabel, setSelectedExpirationLabel] = useState('Add expiration date');
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');
  const [showExpirationOptions, setShowExpirationOptions] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  const expirationOptions = [
    { label: '1 minute', value: 1 },
    { label: '5 minutes', value: 5 },
    { label: '30 minutes', value: 30 },
    { label: '1 hour', value: 60 },
    { label: '5 hours', value: 300 },
  ];

  // Close dropdown when clicking outside
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setShowExpirationOptions(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!originalUrl.trim()) {
      setError('Please enter a valid URL');
      return;
    }

    setIsLoading(true);
    setError('');

    try {
      const response = await shortUrlApi.createShortUrl({
        originalUrl: originalUrl.trim(),
        expiresAt: expiresAt || undefined,
      });
      
      onUrlCreated(response);
      setOriginalUrl('');
      setExpiresAt('');
      setSelectedExpirationLabel('Add expiration date');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to create short URL');
    } finally {
      setIsLoading(false);
    }
  };

  const handleExpirationSelect = (minutes: number | null) => {
    const option = expirationOptions.find(opt => opt.value === minutes);
    setSelectedExpirationLabel(option ? option.label : 'Add expiration date');
    
    if (minutes === null) {
      setExpiresAt('');
    } else {
      const expirationDate = new Date();
      expirationDate.setMinutes(expirationDate.getMinutes() + minutes);
      // Add 10 seconds buffer for very short durations to prevent race conditions
      if (minutes <= 5) {
        expirationDate.setSeconds(expirationDate.getSeconds() + 10);
      }
      // Send full ISO string to ensure proper UTC handling
      setExpiresAt(expirationDate.toISOString());
    }
    setShowExpirationOptions(false);
  };


  return (
    <div>
      <form onSubmit={handleSubmit} className="space-y-6">
        {/* URL Input and Expiration Date Row */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          {/* URL Input */}
          <div className="md:col-span-2">
            <input
              type="url"
              value={originalUrl}
              onChange={(e) => setOriginalUrl(e.target.value)}
              placeholder="Paste the URL to be shortened"
              className="w-full px-4 py-4 border border-gray-300 text-lg placeholder-gray-500 placeholder:text-xl focus:ring-1 focus:ring-purple-500 focus:border-purple-500 focus:outline-none"
              required
            />
          </div>

          {/* Expiration Dropdown */}
          <div className="relative" ref={dropdownRef}>
            <button
              type="button"
              onClick={() => setShowExpirationOptions(!showExpirationOptions)}
              className="w-full px-4 py-4 border border-gray-300 text-lg text-left flex items-center justify-between hover:border-gray-400 focus:ring-2 focus:ring-purple-500 focus:border-transparent"
            >
              <span className="text-xl text-gray-500">
                {selectedExpirationLabel}
              </span>
              <ChevronDown className={`h-5 w-5 text-gray-400 transition-transform ${showExpirationOptions ? 'rotate-180' : ''}`} />
            </button>
            
            {showExpirationOptions && (
              <div className="absolute top-full left-0 right-0 mt-4 bg-white border border-gray-300 z-10">
                {expirationOptions.map((option) => (
                  <button
                    key={option.value}
                    type="button"
                    onClick={() => handleExpirationSelect(option.value)}
                     className="w-full px-4 py-4 text-center text-xl text-gray-500 hover:bg-gray-50 border-b border-gray-300 last:border-b-0"
                  >
                    {option.label}
                  </button>
                ))}
              </div>
            )}
          </div>
        </div>

        {error && (
          <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
            {error}
          </div>
        )}

        {/* Submit Button */}
        <div className="flex justify-start">
          <button
            type="submit"
            disabled={isLoading}
            className="bg-purple-600 text-white py-4 px-8 font-medium text-xl hover:bg-purple-700 focus:ring-2 focus:ring-purple-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
          >
            {isLoading ? 'Creating...' : 'Shorten URL'}
          </button>
        </div>
      </form>
    </div>
  );
};

export default UrlShortener;
