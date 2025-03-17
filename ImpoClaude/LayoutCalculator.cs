

namespace ImpoClaude;

    public static class LayoutCalculator
    {
        public static void CalculateOutputDimensions(int pagesPerSide, float originalWidth, float originalHeight,
            float gap, out float outputWidth, out float outputHeight, out int cols, out int rows)
        {
            // Calcula o número de colunas e linhas com base no número de páginas por lado
            switch (pagesPerSide)
            {
                case 2:
                    cols = 2;
                    rows = 1;
                    break;
                case 4:
                    cols = 2;
                    rows = 2;
                    break;
                case 6:
                    cols = 3;
                    rows = 2;
                    break;
                case 8:
                    cols = 4;
                    rows = 2;
                    break;
                case 9:
                    cols = 3;
                    rows = 3;
                    break;
                case 16:
                    cols = 4;
                    rows = 4;
                    break;
                default:
                    cols = 2;
                    rows = 2;
                    break;
            }

            // Calcula as dimensões da página de saída, incluindo o espaço para a fresa
            outputWidth = (originalWidth * cols) + (gap * (cols - 1));
            outputHeight = (originalHeight * rows) + (gap * (rows - 1));
        }

        public static int[] CalculatePerfectBoundPages(int sheet, int pageCount, int pagesPerSide, bool doubleSided, bool isBackSide = false)
        {
            int[] pages = new int[pagesPerSide];

            // Calcula as páginas para perfect-bound
            // A lógica aqui depende do número total de páginas e folhas
            int totalSheets = doubleSided ? pageCount / (pagesPerSide * 2) : pageCount / pagesPerSide;

            if (!isBackSide)
            {
                // Páginas para a frente da folha
                for (int i = 0; i < pagesPerSide; i++)
                {
                    if (i % 2 == 0)
                    {
                        pages[i] = pageCount - (sheet * pagesPerSide) - i;
                    }
                    else
                    {
                        pages[i] = (sheet * pagesPerSide) + i + 1;
                    }
                }
            }
            else
            {
                // Páginas para o verso da folha
                for (int i = 0; i < pagesPerSide; i++)
                {
                    if (i % 2 == 0)
                    {
                        pages[i] = (sheet * pagesPerSide) + i + 1;
                    }
                    else
                    {
                        pages[i] = pageCount - (sheet * pagesPerSide) - i;
                    }
                }
            }

            return pages;
        }
    }
