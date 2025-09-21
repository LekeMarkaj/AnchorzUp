import React, { useState, useEffect } from 'react';
import { Trash2, QrCode } from 'lucide-react';
import UrlShortener from './components/UrlShortener';
import QrCodeModal from './components/QrCodeModal';
import { ShortUrlResponse, shortUrlApi } from './services/api';
import './App.css';
import logoImage from './assets/AnchorzUpLogo.jpg';

function App() {
  const [urls, setUrls] = useState<ShortUrlResponse[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [qrModalOpen, setQrModalOpen] = useState(false);
  const [selectedUrl, setSelectedUrl] = useState<ShortUrlResponse | null>(null);

  useEffect(() => {
    loadUrls();
  }, []);

  const loadUrls = async () => {
    try {
      const data = await shortUrlApi.getAllShortUrls();
      setUrls(data);
    } catch (error) {
      console.error('Failed to load URLs:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleUrlCreated = (newUrl: ShortUrlResponse) => {
    setUrls(prev => [newUrl, ...prev]);
  };

  const handleUrlDeleted = async (id: string) => {
    if (window.confirm('Are you sure you want to delete this short URL?')) {
      try {
        await shortUrlApi.deleteShortUrl(id);
        setUrls(prev => prev.filter(url => url.id !== id));
      } catch (err) {
        console.error('Failed to delete URL: ', err);
      }
    }
  };

  const handleQrCodeClick = (url: ShortUrlResponse) => {
    setSelectedUrl(url);
    setQrModalOpen(true);
  };

  const handleCloseQrModal = () => {
    setQrModalOpen(false);
    setSelectedUrl(null);
  };

  return (
    <div className="min-h-screen bg-white flex">
      {/* Sidebar */}
      <div className="w-80 bg-gray-50 border-r border-gray-200 flex flex-col">
        {/* Logo */}
        <div className="p-6 border-b border-gray-200">
          <div className="flex items-center justify-center">
            <div className="w-8 h-8 mr-3">
              <img src={logoImage} alt="AnchorzUp Logo" className="w-full h-full object-contain rounded-full" />
            </div>
            <h1 className="text-3xl font-semibold text-black">AnchorzUp</h1>
          </div>
        </div>

        {/* My shortened URLs section */}
        <div className="flex-1 p-6">
          <h2 className="text-lg font-bold text-gray-900 mb-4 text-center">My shortened URLs</h2>
          
          {isLoading ? (
            <div className="text-center py-8">
              <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-purple-600 mx-auto mb-4"></div>
              <p className="text-gray-600 text-sm">Loading...</p>
            </div>
          ) : urls.length === 0 ? (
            <div className="text-center py-8">
              <p className="text-gray-500 text-sm">No URLs yet</p>
            </div>
          ) : (
            <div className="space-y-3">
              {urls.map((url) => (
                <div key={url.id} className="group">
                  <div className="flex items-center justify-between">
                    <a
                      href={url.shortUrl}
                      target="_blank"
                      rel="noopener noreferrer"
                      className="text-blue-600 hover:text-blue-800 text-sm truncate flex-1 mr-2"
                    >
                      {url.shortUrl}
                    </a>
                    <div className="flex items-center space-x-1">
                      <button
                        onClick={() => handleQrCodeClick(url)}
                        className="p-1 text-gray-400 hover:text-purple-600 transition-all"
                        title="Show QR Code"
                      >
                        <QrCode className="h-4 w-4" />
                      </button>
                      <button
                        onClick={() => handleUrlDeleted(url.id)}
                        className="p-1 text-gray-400 hover:text-red-600 transition-all"
                        title="Delete URL"
                      >
                        <Trash2 className="h-4 w-4" />
                      </button>
                    </div>
                  </div>
                  <div className="mt-1">
                    <span style={{ fontSize: '14px', color: '#9bb7f4' }}>
                      This link has been clicked {url.clickCount} times
                    </span>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>

      {/* Main Content */}
      <div className="flex-1 bg-white">
        <div className="p-8">
          <div className="max-w-4xl mx-auto">
            <h2 className="text-3xl font-semibold text-gray-900 mb-8 text-left">URL Shortener</h2>
            <UrlShortener onUrlCreated={handleUrlCreated} />
          </div>
        </div>
      </div>

      {/* QR Code Modal */}
      {selectedUrl && (
        <QrCodeModal
          isOpen={qrModalOpen}
          onClose={handleCloseQrModal}
          qrCodeBase64={selectedUrl.qrCodeBase64}
          shortUrl={selectedUrl.shortUrl}
        />
      )}
    </div>
  );
}

export default App;