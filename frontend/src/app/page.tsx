import { DataTable } from "@/components/data-table";
import Search from "@/components/search-input";
import { Input } from "@/components/ui/input";
import { getPatients } from "@/services";
import { Patient } from "@/types";
import { ColumnDef } from "@tanstack/react-table";

const columns: ColumnDef<Patient>[] = [
  {
    accessorKey: "nome",
    header: "Nome",
  },
  {
    accessorKey: "idade",
    header: "Idade",
  },
  {
    accessorKey: "cpf",
    header: "CPF",
  },
  {
    accessorKey: "rg",
    header: "RG",
  },
  {
    accessorKey: "data_nasc",
    header: "Data de Nascimento",
  },
  {
    accessorKey: "sexo",
    header: "Sexo",
  },

  {
    accessorKey: "email",
    header: "Email",
  },

  {
    accessorKey: "celular",
    header: "Celular",
  },
  {
    accessorKey: "altura",
    header: "Altura",
  },
  {
    accessorKey: "peso",
    header: "Peso",
  },
];

export default async function Patients({
  searchParams,
}: {
  searchParams?: {
    query?: string;
    page?: string;
  };
}) {
  const patients = await getPatients();
  const query = searchParams?.query || "";

  return (
    <main className="flex max-h-screen flex-col items-center p-24 gap-4">
      <section className="flex-1 w-full">
        <Search placeholder="Buscar pacientes" />
      </section>
      <section className="flex-1">
        <DataTable columns={columns} data={patients} filterText={query} />
      </section>
    </main>
  );
}
