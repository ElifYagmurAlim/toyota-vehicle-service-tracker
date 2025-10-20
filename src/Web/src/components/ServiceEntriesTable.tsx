import type { PaginatedResult, ServiceEntry } from '../types';

interface ServiceEntriesTableProps {
  data?: PaginatedResult<ServiceEntry>;
  isLoading: boolean;
  pageNumber: number;
  onPageChange: (page: number) => void;
}

const ServiceEntriesTable = ({ data, isLoading, pageNumber, onPageChange }: ServiceEntriesTableProps) => {
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
              </tr>
            ))}
          </tbody>
        </table>
      </div>

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