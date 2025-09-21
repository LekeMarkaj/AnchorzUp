import React from 'react';
import { X, Download } from 'lucide-react';

interface QrCodeModalProps {
  isOpen: boolean;
  onClose: () => void;
  qrCodeBase64: string;
  shortUrl: string;
}

const QrCodeModal: React.FC<QrCodeModalProps> = ({ isOpen, onClose, qrCodeBase64, shortUrl }) => {
  if (!isOpen) return null;

  const handleDownload = () => {
    const link = document.createElement('a');
    link.href = `data:image/png;base64,${qrCodeBase64}`;
    link.download = `qr-code-${shortUrl.split('/').pop()}.png`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  const handleBackdropClick = (e: React.MouseEvent) => {
    if (e.target === e.currentTarget) {
      onClose();
    }
  };

  return (
    <div 
      className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
      onClick={handleBackdropClick}
    >
      <div className="bg-white rounded-lg p-6 max-w-md w-full mx-4 shadow-xl">
        {/* Header */}
        <div className="flex items-center justify-between mb-4">
          <h3 className="text-lg font-semibold text-gray-900">QR Code</h3>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-gray-600 transition-colors"
          >
            <X className="h-6 w-6" />
          </button>
        </div>

        {/* QR Code Display */}
        <div className="flex flex-col items-center space-y-4">
          <div className="bg-white p-4 rounded-lg border-2 border-gray-200">
            <img
              src={`data:image/png;base64,${qrCodeBase64}`}
              alt="QR Code"
              className="w-48 h-48"
            />
          </div>

          {/* URL Display */}
          <div className="text-center">
            <p className="text-sm text-gray-600 mb-2">Scan to visit:</p>
            <p className="text-blue-600 font-medium break-all">{shortUrl}</p>
          </div>

          {/* Download Button */}
          <button
            onClick={handleDownload}
            className="flex items-center space-x-2 bg-purple-600 text-white px-4 py-2 rounded-lg hover:bg-purple-700 transition-colors"
          >
            <Download className="h-4 w-4" />
            <span>Download QR Code</span>
          </button>
        </div>
      </div>
    </div>
  );
};

export default QrCodeModal;
