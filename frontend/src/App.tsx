import React, { useState, useEffect } from 'react';
import { Trash2, QrCode, Menu, X } from 'lucide-react';
import UrlShortener from './components/UrlShortener';
import QrCodeModal from './components/QrCodeModal';
import DeleteConfirmationModal from './components/DeleteConfirmationModal';
import SuccessToast from './components/SuccessToast';
import ErrorToast from './components/ErrorToast';
import { ShortUrlResponse, shortUrlApi } from './services/api';
import './App.css';
import logoImage from './assets/AnchorzUpLogo.jpg';

function App() {
  const [urls, setUrls] = useState<ShortUrlResponse[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [qrModalOpen, setQrModalOpen] = useState(false);
  const [selectedUrl, setSelectedUrl] = useState<ShortUrlResponse | null>(null);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [urlToDelete, setUrlToDelete] = useState<ShortUrlResponse | null>(null);
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const [showSuccessToast, setShowSuccessToast] = useState(false);
  const [successMessage, setSuccessMessage] = useState('');
  const [showErrorToast, setShowErrorToast] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');

  useEffect(() => {
    loadUrls();
  }, []);

  // Auto-close sidebar on desktop screens
  useEffect(() => {
    const handleResize = () => {
      if (window.innerWidth >= 1024) { // lg breakpoint
        setSidebarOpen(false);
      }
    };

    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
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
    setSuccessMessage('Short URL created successfully!');
    setShowSuccessToast(true);
  };

  const handleCloseSuccessToast = () => {
    setShowSuccessToast(false);
    setSuccessMessage('');
  };

  const handleCloseErrorToast = () => {
    setShowErrorToast(false);
    setErrorMessage('');
  };

  const handleError = (error: string) => {
    setErrorMessage(error);
    setShowErrorToast(true);
  };

  const handleDeleteClick = (url: ShortUrlResponse) => {
    setUrlToDelete(url);
    setDeleteModalOpen(true);
  };

  const handleDeleteConfirm = async () => {
    if (!urlToDelete) return;
    
    try {
      await shortUrlApi.deleteShortUrl(urlToDelete.id);
      setUrls(prev => prev.filter(url => url.id !== urlToDelete.id));
      setSuccessMessage('Short URL deleted successfully!');
      setShowSuccessToast(true);
    } catch (err) {
      console.error('Failed to delete URL: ', err);
      setErrorMessage('Failed to delete short URL. Please try again.');
      setShowErrorToast(true);
    }
  };

  const handleCloseDeleteModal = () => {
    setDeleteModalOpen(false);
    setUrlToDelete(null);
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
      {/* Mobile Menu Button */}
      <button
        onClick={() => setSidebarOpen(!sidebarOpen)}
        className="lg:hidden fixed top-4 left-4 z-50 p-2 bg-white rounded-lg shadow-md border border-gray-200 hover:bg-gray-50 transition-colors"
      >
        {sidebarOpen ? <X size={24} /> : <Menu size={24} />}
      </button>

      {/* Mobile Overlay */}
      {sidebarOpen && (
        <div
          className="lg:hidden fixed inset-0 bg-black bg-opacity-50 z-40"
          onClick={() => setSidebarOpen(false)}
        />
      )}

      {/* Sidebar */}
      <div className={`
        fixed lg:static inset-y-0 left-0 z-50
        w-80 bg-gray-50 border-r border-gray-200 flex flex-col
        transform transition-transform duration-300 ease-in-out
        ${sidebarOpen ? 'translate-x-0' : '-translate-x-full lg:translate-x-0'}
      `}>
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
        <div className="flex-1 p-6 overflow-y-auto">
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
                        onClick={() => handleDeleteClick(url)}
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
      <div className="flex-1 bg-white lg:ml-0">
        <div className="p-8 pt-16 lg:pt-8">
          <div className="max-w-4xl mx-auto">
            <h2 className="text-3xl font-semibold text-gray-900 mb-8 text-left">URL Shortener</h2>
            <UrlShortener onUrlCreated={handleUrlCreated} onError={handleError} />
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

      {/* Delete Confirmation Modal */}
      {urlToDelete && (
        <DeleteConfirmationModal
          isOpen={deleteModalOpen}
          onClose={handleCloseDeleteModal}
          onConfirm={handleDeleteConfirm}
          shortUrl={urlToDelete.shortUrl}
          clickCount={urlToDelete.clickCount}
        />
      )}

      {/* Success Toast */}
      <SuccessToast
        isVisible={showSuccessToast}
        message={successMessage}
        onClose={handleCloseSuccessToast}
        duration={3000}
      />

      {/* Error Toast */}
      <ErrorToast
        isVisible={showErrorToast}
        message={errorMessage}
        onClose={handleCloseErrorToast}
        duration={5000}
      />
    </div>
  );
}

export default App;