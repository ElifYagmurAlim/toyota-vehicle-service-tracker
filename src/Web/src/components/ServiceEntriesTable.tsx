import { useState } from 'react';
import type { PaginatedResult, ServiceEntry } from '../types';

interface ServiceEntriesTableProps {
  data?: PaginatedResult<ServiceEntry>;
  isLoading: boolean;
  pageNumber: number;
  onPageChange: (page: number) => void;
}

const ServiceEntriesTable = ({ data, isLoading, pageNumber, onPageChange }: ServiceEntriesTableProps) => {
  const [selectedEntry, setSelectedEntry] = useState<ServiceEntry | null>(null);

  if (isLoading) {
    return (
      <div className="p-8 text-center">
        <div className="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-red-600"></div>
        <p className="mt-2 text-gray-600">Yükleniyor...</p>
      </div>
    );
  }

  if (!data || data.items.length === 0) {
    return (
      <div className="p-8 text-center text-gray-500">
        Henüz servis girişi bulunmamaktadır.
      </div>
    );
  }

  return (
    <>
      <div className="overflow-x-auto">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Plaka
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Marka & Model
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                KM
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Model Yılı
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Servis Tarihi
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Garanti
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Şehir
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                İşlemler
              </th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {data.items.map((entry) => (
              <tr key={entry.id} className="hover:bg-gray-50 transition">
                <td className="px-6 py-4 whitespace-nowrap">
                  <span className="font-semibold text-gray-900">{entry.licensePlate}</span>
                </td>
                <td className="px-6 py-4">
                  <div className="text-sm text-gray-900">{entry.brandName}</div>
                  <div className="text-sm text-gray-500">{entry.modelName}</div>
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                  {entry.kilometers.toLocaleString('tr-TR')} km
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                  {entry.modelYear || '-'}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                  {new Date(entry.serviceDate).toLocaleDateString('tr-TR')}
                </td>
                <td className="px-6 py-4 whitespace-nowrap">
                  {entry.hasWarranty !== null && entry.hasWarranty !== undefined ? (
                    <span
                      className={`px-2 py-1 inline-flex text-xs leading-5 font-semibold rounded-full ${
                        entry.hasWarranty
                          ? 'bg-green-100 text-green-800'
                          : 'bg-red-100 text-red-800'
                      }`}
                    >
                      {entry.hasWarranty ? 'Var' : 'Yok'}
                    </span>
                  ) : (
                    <span className="text-gray-400">-</span>
                  )}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                  {entry.serviceCity || '-'}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                  <button
                    onClick={() => setSelectedEntry(entry)}
                    className="inline-flex items-center px-3 py-1.5 bg-red-600 text-white text-xs font-medium rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition duration-150"
                  >
                    <svg className="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                    </svg>
                    Detay
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Servis Notu Modal */}
      {selectedEntry && (
        <div 
          className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50 flex items-center justify-center p-4"
          onClick={() => setSelectedEntry(null)}
        >
          <div 
            className="relative bg-white rounded-lg shadow-xl max-w-md w-full"
            onClick={(e) => e.stopPropagation()}
          >
            {/* Modal Header */}
            <div className="px-6 py-4 border-b border-gray-200">
              <h3 className="text-lg font-medium text-gray-900">
                Servis Detayı
              </h3>
              <p className="text-sm text-gray-600">
                {selectedEntry.licensePlate} - {selectedEntry.brandName} {selectedEntry.modelName}
              </p>
            </div>
            
            {/* Modal Body */}
            <div className="px-6 py-4">
              <div className="space-y-3">
                <div>
                  <dt className="text-sm font-medium text-gray-500">Servis Tarihi:</dt>
                  <dd className="text-sm text-gray-900">
                    {new Date(selectedEntry.serviceDate).toLocaleDateString('tr-TR')}
                  </dd>
                </div>
                
                <div>
                  <dt className="text-sm font-medium text-gray-500">Şehir:</dt>
                  <dd className="text-sm text-gray-900">{selectedEntry.serviceCity || '-'}</dd>
                </div>
                
                <div>
                  <dt className="text-sm font-medium text-gray-500">Kilometre:</dt>
                  <dd className="text-sm text-gray-900">
                    {selectedEntry.kilometers.toLocaleString('tr-TR')} km
                  </dd>
                </div>
                
                <div>
                  <dt className="text-sm font-medium text-gray-500">Garanti Durumu:</dt>
                  <dd className="text-sm text-gray-900">
                    {selectedEntry.hasWarranty !== null && selectedEntry.hasWarranty !== undefined 
                      ? (selectedEntry.hasWarranty ? 'Garantili' : 'Garanti Yok') 
                      : '-'
                    }
                  </dd>
                </div>
                
                <div>
                  <dt className="text-sm font-medium text-gray-500">Servis Notu:</dt>
                  <dd className="text-sm text-gray-900 p-3 bg-gray-50 rounded-md min-h-[80px] max-h-[200px] overflow-y-auto">
                    {selectedEntry.serviceNote || 'Servis notu bulunmamaktadır.'}
                  </dd>
                </div>
              </div>
            </div>
            
            {/* Modal Footer */}
            <div className="px-6 py-4 border-t border-gray-200 flex justify-end">
              <button
                onClick={() => setSelectedEntry(null)}
                className="px-4 py-2 bg-gray-200 text-gray-800 text-sm font-medium rounded-md hover:bg-gray-300 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-1 transition duration-150"
              >
                Kapat
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Pagination */}
      <div className="px-6 py-4 border-t border-gray-200 flex items-center justify-between">
        <div className="text-sm text-gray-700">
          Toplam <span className="font-medium">{data.totalCount}</span> kayıt içinden{' '}
          <span className="font-medium">{(pageNumber - 1) * data.pageSize + 1}</span> -{' '}
          <span className="font-medium">
            {Math.min(pageNumber * data.pageSize, data.totalCount)}
          </span>{' '}
          arası gösteriliyor
        </div>
        <div className="flex gap-2">
          <button
            onClick={() => onPageChange(pageNumber - 1)}
            disabled={pageNumber === 1}
            className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed transition"
          >
            Önceki
          </button>
          <span className="px-4 py-2 text-sm font-medium text-gray-700">
            Sayfa {pageNumber} / {data.totalPages}
          </span>
          <button
            onClick={() => onPageChange(pageNumber + 1)}
            disabled={pageNumber >= data.totalPages}
            className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed transition"
          >
            Sonraki
          </button>
        </div>
      </div>
    </>
  );
};

export default ServiceEntriesTable;